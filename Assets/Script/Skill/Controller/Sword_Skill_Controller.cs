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

    [Header("Bounce info")]
    [SerializeField] private float maxBounceDistance = 20;
    [SerializeField] private float bounceSpeed = 20;
    [SerializeField] private float bounceAttackDistance = 0.15f;
    private List<Transform> enemyTarget;
    private bool isBouncing;
    private int bounceAmount;
    private int targetIndex = 0;

    [Header("Peirce info")]
    [SerializeField] private int peirceAmount;

    private int swordAttackDir;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 _dir , float _gravityScale , Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if(peirceAmount > 0) 
            anim.SetBool("Rotation" ,false);
        else
            anim.SetBool("Rotation" ,true);
    }

    public void SetUpBounce(bool _isBouncing , int _bounceAmount)
    {
        isBouncing= _isBouncing;
        bounceAmount = _bounceAmount;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPeirce(int _peirceAmount)
    {
        peirceAmount = _peirceAmount;
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

            if (Vector2.Distance(transform.position, targetPosition) < bounceAttackDistance)
            {
                var enemy = enemyTarget[targetIndex].GetComponent<Enemy>();
                if (enemy != null)
                    enemy.Damage(swordAttackDir);

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

        collision.GetComponent<Enemy>()?.Damage(swordAttackDir);

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

        StuctInto(collision);
    }

    private void StuctInto(Collider2D collision)
    {
        if (peirceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            peirceAmount--;
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
