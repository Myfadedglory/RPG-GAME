using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;
    private float catchSwordDistance = 1f;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private int swordAttackDir;
    private float freezeDuration;

    [Header("Bounce info")]
    private float maxBounceDistance = 20;
    private float bounceSpeed = 20;
    private List<Transform> enemyTarget;
    private bool isBouncing;
    private int bounceAmount;
    private int targetIndex = 0;

    [Header("Peirce info")]
    private int peirceAmount;

    [Header("Spin info")]
    private float maxSpinDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float spinMoveSpeed;

    [Header("Hit info")]
    private float hitDistance = 0.15f;
    private float hitCoolDown;
    private float hitTimer;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(
        Vector2 _dir ,
        float _gravityScale ,
        float _hitDistance,
        float _freezeDuration,
        Player _player)
    {
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

    public void SetUpBounce(
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

    public void SetUpPeirce(int _peirceAmount)
    {
        peirceAmount = _peirceAmount;
    }

    public void SetUpSpin(
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

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (rb.velocity.x != 0)
            swordAttackDir = rb.velocity.x > 0 ? 1 : -1;

        if (canRotate)
            transform.right = rb.velocity;

        ReturnLogic();

        BounceLogic();

        SpinLogic();

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

    private void SpinLogic()
    {
        if(isSpinning)
        {
            if(Vector2.Distance(transform.position , player.transform.position) > maxSpinDistance && !wasStopped)
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

                if(hitTimer < 0)
                    hitTimer = hitCoolDown;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitDistance);

                foreach (var hit in colliders)
                {
                    hit.GetComponent<Enemy>()?.Damage();
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (!wasStopped)
        {
            if(collision.GetComponent<Enemy>() != null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Damage(swordAttackDir);
                enemy.StartCoroutine("FreezeTimeFor", freezeDuration);
            }

        }
        else
            collision.GetComponent<Enemy>().Damage();

        SetUpBounceTarget(collision);

        StuctInto(collision);
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

    private void StuctInto(Collider2D collision)
    {
        if (peirceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            peirceAmount--;
            return;
        }

        if( isSpinning && collision.GetComponent<Enemy>()!= null)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
