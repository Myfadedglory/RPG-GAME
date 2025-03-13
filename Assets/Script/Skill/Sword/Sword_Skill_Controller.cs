using System;
using System.Collections.Generic;
using Script.Utilities;
using UnityEngine;

namespace Script.Skill.Sword
{
    public class SwordSkillController : MonoBehaviour
    {
        private static readonly int Rotation = Animator.StringToHash("Rotation");
        private SwordType swordType;
        private SwordConfig config;

        private Animator anim;
        private Rigidbody2D rb;
        private CircleCollider2D cd;
        private Entity.Player.Player player;

        private bool canRotate = false;
        private bool isReturning;
        private int swordAttackDir;

        private readonly List<Transform> enemyTarget = new();
        private bool isBouncing;
        private int bounceAmount;
        private int targetIndex = 0;

        private int peirceAmount;

        private float spinTimer;
        private bool wasStopped;
        private bool isSpinning;

        private float hitTimer;

        private List<Vector2> returnPath;
        private int currentPathIndex;
        [SerializeField] private float gridSize = 1f; // 网格大小
        private float lastPathUpdateTime;
        [SerializeField] private float pathUpdateInterval = 0.5f; // 每0.5秒更新一次路径

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<CircleCollider2D>();
        }

        public void SetUpSword(
            SwordType swordType,
            Vector2 dir,
            float gravityScale,
            SwordConfig config,
            Entity.Player.Player player)
        {
            this.swordType = swordType;
            this.player = player;
            this.config = config;

            rb.velocity = dir;
            rb.gravityScale = gravityScale;
            
            anim.SetBool(Rotation, peirceAmount <= 0);

            InitializeSword();
        }

        private void InitializeSword()
        {
            switch (swordType)
            {
                case SwordType.Bounce:
                    isBouncing = true;
                    bounceAmount = config.bounceAmount;
                    break;
                case SwordType.Spin:
                    isSpinning = true;
                    break;
                case SwordType.Pierce:
                    peirceAmount = config.pierceAmount;
                    canRotate = false;
                    break;
                case SwordType.Regular:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (rb.velocity.x != 0)
                swordAttackDir = rb.velocity.x > 0 ? 1 : -1;

            if (canRotate)
                transform.right = rb.velocity;

            ReturnLogic();

            switch (swordType)
            {
                case SwordType.Bounce:
                    BounceLogic();
                    break;
                case SwordType.Spin:
                    SpinLogic();
                    break;
                case SwordType.Regular:
                case SwordType.Pierce:
                default:
                    break;
            }
        }

        #region Bounce

        private void BounceLogic()
        {
            if (!isBouncing || enemyTarget.Count <= 0) return;

            Vector2 targetPosition = enemyTarget[targetIndex].position;

            swordAttackDir = transform.position.x < targetPosition.x ? 1 : -1;

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                config.bounceSpeed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, targetPosition) < config.rotationSwordHitDistance)) return;

            var enemy = enemyTarget[targetIndex].GetComponent<Entity.Enemy.Enemy>();
            enemy.Damage(player.Stats, new Vector2(swordAttackDir, 0));
            enemy.FreezeTimeFor(config.freezeDuration);

            bounceAmount--;

            if (bounceAmount < 0)
            {
                isBouncing = false;
                isReturning = true;
                return;
            }

            targetIndex = FindClosestEnemyIndex();

            if (targetIndex != -1) return;

            isBouncing = false;
            isReturning = true;
        }

        private void HandleBounce(Collider2D collision)
        {
            AttackEnemy(collision);

            SetUpBounceTarget(collision);

            PhysicAttribute();

            if (isBouncing && enemyTarget.Count > 0)
                return;

            AnimationAttribute(collision);
        }

        private void SetUpBounceTarget(Collider2D collision)
        {
            if (collision.GetComponent<Entity.Enemy.Enemy>() == null || !isBouncing || enemyTarget.Count > 0) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, config.maxBounceDistance);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Entity.Enemy.Enemy>() != null)
                    enemyTarget.Add(hit.transform);
            }
        }

        private int FindClosestEnemyIndex()
        {
            var closestDistance = Mathf.Infinity;

            var closestIndex = -1;

            for (var i = 0; i < enemyTarget.Count; i++)
            {
                if (i == targetIndex)
                    continue;

                var distance = Vector2.Distance(transform.position, enemyTarget[i].position);

                if (distance >= closestDistance) continue;

                closestDistance = distance;

                closestIndex = i;
            }

            return closestIndex;
        }

        #endregion

        #region Pierce

        private void HandlePierce(Collider2D collision)
        {
            AttackEnemy(collision);

            if (peirceAmount > 0 && collision.GetComponent<Entity.Enemy.Enemy>() != null)
            {
                peirceAmount--;

                return;
            }

            PhysicAttribute();

            AnimationAttribute(collision);
        }

        #endregion

        #region Spin

        private void SpinLogic()
        {
            if (!isSpinning) return;

            if (Vector2.Distance(transform.position, player.transform.position) > config.maxSpinDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (!wasStopped) return;

            spinTimer -= Time.deltaTime;

            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x + swordAttackDir, transform.position.y),
                config.spinMoveSpeed * Time.deltaTime);

            if (spinTimer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }

            hitTimer -= Time.deltaTime;

            if (hitTimer < 0)
                hitTimer = config.hitCoolDown;

            var colliders = Physics2D.OverlapCircleAll(transform.position, config.rotationSwordHitDistance);

            foreach (var hit in colliders)
            {
                hit.GetComponent<Entity.Enemy.Enemy>()?.Damage(player.Stats);
            }
        }

        private void HandleSpin(Collider2D collision)
        {
            if (!wasStopped)
                AttackEnemy(collision);
            else
                collision.GetComponent<Entity.Enemy.Enemy>().Damage(player.Stats);

            if (isSpinning && collision.GetComponent<Entity.Enemy.Enemy>() != null)
            {
                StopWhenSpinning();

                return;
            }

            PhysicAttribute();

            AnimationAttribute(collision);
        }

        private void StopWhenSpinning()
        {
            wasStopped = true;

            rb.constraints = RigidbodyConstraints2D.FreezePosition;

            spinTimer = config.spinDuration;
        }

        #endregion

        #region Sword Return

        public void ReturnSword()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            transform.parent = null;
            
            // 计算路径
            returnPath = AStar.FindPath(transform.position, player.transform.position, config.whatIsObstacle, gridSize);
            currentPathIndex = 0;

            if (returnPath == null || returnPath.Count == 0)
            {
                Debug.LogWarning("No path found! Returning in a straight line.");
            }

            isReturning = true;
        }

        private void ReturnLogic()
        {
            if (!isReturning) return;
            
            

            // 检查是否需要更新路径
            if (Time.time - lastPathUpdateTime > pathUpdateInterval)
            {
                lastPathUpdateTime = Time.time;

                // 如果角色移动了，重新计算路径
                if (returnPath is { Count: > 0 })
                {
                    var distanceToLastPathPoint = Vector2.Distance(returnPath[^1], player.transform.position);

                    // 如果当前路径的终点离角色太远，重新计算路径
                    if (distanceToLastPathPoint > gridSize * 2)
                    {
                        returnPath = AStar.FindPath(transform.position, player.transform.position, config.whatIsObstacle, gridSize);
                        currentPathIndex = 0;

                        if (returnPath == null || returnPath.Count == 0)
                        {
                            Debug.LogWarning("No path found! Returning in a straight line.");
                        }
                    }
                }
            }

            // 移动逻辑（保持不变）
            if (returnPath is { Count: > 0 } && currentPathIndex < returnPath.Count)
            {
                // 移动到下一个路径点
                var targetPosition = returnPath[currentPathIndex];
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, config.returnSpeed * Time.deltaTime);
                transform.right =new Vector2( -rb.velocity.x, -rb.velocity.y);    
                
                // 更新旋转方向
                var dir = (targetPosition - (Vector2)transform.position).normalized;
                if (dir != Vector2.zero && canRotate)
                    transform.right = dir;

                // 检查是否到达当前路径点
                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentPathIndex++;
                }
            }
            else
            {
                // 如果没有路径或路径走完，直接直线返回
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, config.returnSpeed * Time.deltaTime);
            }

            // 最终接近玩家时直接吸附
            if (Vector2.Distance(transform.position, player.transform.position) < config.catchSwordDistance)
                player.CatchTheSword();
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isReturning)
                return;

            switch (swordType)
            {
                case SwordType.Bounce:
                    HandleBounce(collision);
                    break;
                case SwordType.Pierce:
                    HandlePierce(collision);
                    break;
                case SwordType.Spin:
                    HandleSpin(collision);
                    break;
                case SwordType.Regular:
                default:
                    AttackEnemy(collision);
                    PhysicAttribute();
                    AnimationAttribute(collision);
                    break;
            }
        }

        private void AttackEnemy(Collider2D collision)
        {
            if (collision.GetComponent<Entity.Enemy.Enemy>() == null) return;

            var enemy = collision.GetComponent<Entity.Enemy.Enemy>();

            enemy.Damage(player.Stats, new Vector2(swordAttackDir, 0));

            enemy.FreezeTimeFor(config.freezeDuration);
        }

        private void PhysicAttribute()
        {
            canRotate = false;

            cd.enabled = false;

            rb.isKinematic = true;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void AnimationAttribute(Collider2D collision)
        {
            anim.SetBool(Rotation, false);

            transform.parent = collision.transform;
        }
    }
}