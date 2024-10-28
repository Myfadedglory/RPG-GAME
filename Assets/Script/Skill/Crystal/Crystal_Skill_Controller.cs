using System;
using Script.Player;
using UnityEngine;

namespace Script.Skill.Crystal
{
    public class Crystal_Skill_Controller : MonoBehaviour
    {
        private static readonly int Explode = Animator.StringToHash("Explode");
        private Animator anim => GetComponent<Animator>();
        private CircleCollider2D cd => GetComponent<CircleCollider2D>();

        private float crystalExitTimer;

        private bool canExplode;
        private bool canMove;
        private bool canGrow;

        private float moveSpeed;
        private Vector2 maxSize;
        private float growSpeed;

        private readonly float crystalExplodeDistance = 0.5f;

        private Transform followTarget;

        public void SetUpCrystal(
            bool canMove, 
            bool canExplode, 
            float growSpeed,
            float moveSpeed,
            Vector2 maxSize,
            bool chooseRandomTarget,
            float crystalDuration,
            float crystalDetectDistance,
            Func<Transform, float, Transform> ChooseClosestEnemy,
            Func<Transform, float, Transform> ChooseRandomEnemy
        )
        {
            crystalExitTimer = crystalDuration;
            this.canExplode = canExplode;
            this.canMove = canMove;
            this.growSpeed = growSpeed;
            this.moveSpeed = moveSpeed;
            this.maxSize = maxSize;
   
            followTarget = chooseRandomTarget ? ChooseRandomEnemy(transform, crystalDetectDistance) : ChooseClosestEnemy(transform, crystalDetectDistance);
        }

        private void Update()
        {
            crystalExitTimer -= Time.deltaTime;

            if (crystalExitTimer < 0)
                CrystalExitTimeOver();

            if (canGrow)
                transform.localScale = Vector2.Lerp(transform.localScale, maxSize, growSpeed * Time.deltaTime);
            
            if (!canMove || !followTarget) return;
            
            transform.position = Vector2.MoveTowards(
                transform.position,
                followTarget.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, followTarget.position) < crystalExplodeDistance)
                CrystalExitTimeOver();
        }

        private void AnimationExplodeEvent()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

            foreach(var hit in colliders)
            {
                if (hit.GetComponent<Enemy.Enemy>() == null) continue;
                
                var enemy = hit.GetComponent<Enemy.Enemy>();
                if (enemy.transform.position.x <= transform.position.x)
                    enemy.Damage(PlayerManger.instance.player.Stats, (int)enemy.left.x);
                else if (enemy.transform.position.x > transform.position.x)
                    enemy.Damage(PlayerManger.instance.player.Stats, (int)enemy.right.x);
            }
        }

        public void CrystalExitTimeOver()
        {
            if (canExplode)
            {
                canMove = false;
                canGrow = true;
                anim.SetTrigger(Explode);
            }
            else
                SelfDestroy();
        }

        private void SelfDestroy() => Destroy(gameObject);
    }
}
