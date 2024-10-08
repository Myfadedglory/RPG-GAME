using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;
    private float catchSwordDistance = 1f;
    private SwordType swordType;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private int swordAttackDir;
    private float freezeDuration;

    private float maxBounceDistance;
    private float bounceSpeed;
    private List<Transform> enemyTarget;
    private bool isBouncing;
    private int bounceAmount;
    private int targetIndex = 0;

    private int peirceAmount;

    private float maxSpinDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float spinMoveSpeed;

    private float hitDistance;
    private float hitCoolDown;
    private float hitTimer;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(
        SwordType _swordType,
        Vector2 _dir ,
        float _gravityScale ,
        float _hitDistance,
        float _freezeDuration,
        Player _player)
    {
        swordType = _swordType;
        player = _player;

        hitDistance = _hitDistance;
        freezeDuration = _freezeDuration;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if(peirceAmount > 0) 
            anim.SetBool("Rotation" ,false);
        else
            anim.SetBool("Rotation" ,true);
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
            default:
                break;
        }
    }

    #region Bounce

    public void BounceAttribute(
        bool _isBouncing ,
        int _bounceAmount ,
        float _maxBounceDistance ,
        float _bounceSpeed
        )
    {
        isBouncing= _isBouncing;
        bounceAmount = _bounceAmount;
        maxBounceDistance = _maxBounceDistance;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            Vector2 targetPosition = enemyTarget[targetIndex].position;

            swordAttackDir = transform.position.x < targetPosition.x ? 1 : -1;

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < hitDistance)
            {

                Enemy enemy = enemyTarget[targetIndex].GetComponent<Enemy>();
                enemy.Damage(swordAttackDir);
                enemy.StartCoroutine("FreezeTimeFor", freezeDuration);

                bounceAmount--;

                if (bounceAmount < 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    return;
                }

                targetIndex = FindClosestEnemyIndex();

                if (targetIndex == -1)
                {
                    isBouncing = false;
                    isReturning = true;
                    return;
                }
            }
        }
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
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxBounceDistance);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private int FindClosestEnemyIndex()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < enemyTarget.Count; i++)
        {
            if (i == targetIndex)
                continue;

            float distance = Vector2.Distance(transform.position, enemyTarget[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    #endregion

    #region Pierce

    public void PeirceAttribute(int _peirceAmount)
    {
        peirceAmount = _peirceAmount;
    }

    private void HandlePierce(Collider2D collision)
    {
        AttackEnemy(collision);

        if (peirceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            peirceAmount--;
            return;
        }

        PhysicAttribute();

        AnimationAttribute(collision);

    }

    #endregion

    #region Spin

    public void SpinAttribute(
        bool _isSpinning ,
        float _maxSpinDistance , 
        float _spinDuration , 
        float _hitCoolDown , 
        float _spinMoveSpeed
        )
    {
        isSpinning = _isSpinning;
        maxSpinDistance = _maxSpinDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
        spinMoveSpeed = _spinMoveSpeed;

    }


    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxSpinDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(
                    transform.position,
                    new Vector2(transform.position.x + swordAttackDir, transform.position.y),
                    spinMoveSpeed * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                    hitTimer = hitCoolDown;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitDistance);

                foreach (var hit in colliders)
                {
                    hit.GetComponent<Enemy>()?.Damage();
                }
            }
        }
    }

    private void HandleSpin(Collider2D collision)
    {
        if (!wasStopped)
        {
            AttackEnemy(collision);
        }
        else
            collision.GetComponent<Enemy>().Damage();

        if (isSpinning && collision.GetComponent<Enemy>() != null)
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
        spinTimer = spinDuration;
    }

    #endregion

    #region Sword Return

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void ReturnLogic()
    {
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < catchSwordDistance)
                player.CatchTheSword();
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        switch(swordType)
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
            default:
                AttackEnemy(collision);
                PhysicAttribute();
                AnimationAttribute(collision);
                break;
        }
    }

    private void AttackEnemy(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Damage(swordAttackDir);
            enemy.StartCoroutine("FreezeTimeFor", freezeDuration);
        }
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
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
