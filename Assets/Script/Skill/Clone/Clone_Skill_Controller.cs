using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;

    private float clonerDetectDistance;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadious = 0.8f;

    private int cloneFacingDir = 1;
    private Transform closestEnemy;
    private Func<Transform, float, Transform> FindClosestEnemy;

    private bool canDuplicateClone;
    private float chanceToDuplcate;

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
        Transform newTransform,
        float cloneDuration,
        float clonerDetectDistance,
        bool canAttack,
        Func<Transform, float, Transform> FindClosestEnemy,
        bool canDuplicateClone,
        float chanceToDuplcate,
        Vector3 _offset   
    )  
    {
        Vector3 offset = _offset;

        transform.position = newTransform.position + offset;

        cloneTimer = cloneDuration;

        this.FindClosestEnemy = FindClosestEnemy;

        this.canDuplicateClone = canDuplicateClone;

        this.chanceToDuplcate = chanceToDuplcate;

        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        this.clonerDetectDistance = clonerDetectDistance;

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
            {
                hit.GetComponent<Enemy>().Damage(cloneFacingDir);

                if (canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplcate * 100)
                    {
                        SkillManger.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * cloneFacingDir, 0));
                    }
                }
            }
        }
    }

    private void FacingToClosestTarget()
    {
        closestEnemy = FindClosestEnemy(transform, clonerDetectDistance);

        if(closestEnemy != null)
        {
            if (closestEnemy.position.x < transform.position.x)
            {
                cloneFacingDir = -1;

                transform.Rotate(0, 180, 0);
            }
        }
    }
}
