using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private float clonerDetectDistance;
    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadious = 0.8f;

    private int cloneFacingDir;
    private Transform closestEnemy;
    private Func<Transform, float, Transform> findClosestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {    
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if(sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetUpClone(
    Transform _newTransform,
    float _cloneDuration,
    float _clonerDetectDistance,
    bool _canAttack,
    Func<Transform, float, Transform> findClosestEnemy,
    Vector3? _offset = null     //偏移量 使用可空类型
    )  
    {
        Vector3 offset = _offset ?? Vector3.zero;  // 如果没有传入，使用 Vector3.zero

        this.findClosestEnemy = findClosestEnemy;

        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        transform.position = _newTransform.position + offset;

        clonerDetectDistance = _clonerDetectDistance;

        cloneTimer = _cloneDuration;

        FacingToClosestTarget();
    }


    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadious);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage(cloneFacingDir);
        }
    }

    private void FacingToClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, clonerDetectDistance);

        float closestDistance = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToPlayer < closestDistance)
                {
                    clonerDetectDistance = distanceToPlayer;

                    closestEnemy = hit.transform;
                }
            }
        }
        if(closestEnemy != null)
        {
            if (closestEnemy.position.x < transform.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
