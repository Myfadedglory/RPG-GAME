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
    private int amountOfBounce;
    private int targetIndex = 0;

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

        anim.SetBool("Rotation" ,true);
    }

    public void SetUpBounce(bool _isBouncing , int _amountOfBounce)
    {
        isBouncing= _isBouncing;
        amountOfBounce = _amountOfBounce;

        enemyTarget = new List<Transform>();
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
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyTarget[targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < bounceAttackDistance)
            {
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce < 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

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
