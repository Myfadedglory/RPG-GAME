using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExitTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private Vector2 maxSize;
    private float growSpeed;
    private float detectEnemyDistance = 20;
    private float crystalExplodeDistance = 1;

    private Func<Transform, float, Transform> findClosestEnemy;


    public void SetUpCrystal(
        float crystalDuration,
        bool canExplode, 
        bool canMove, 
        float growSpeed,
        float moveSpeed,
        Vector2 maxSize,
        Func<Transform, float, Transform> findClosestEnemy
    )
    {
        crystalExitTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.maxSize = maxSize;
        this.findClosestEnemy = findClosestEnemy;
    }

    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;

        if (crystalExitTimer < 0)
            CrystalExitTimeOver();

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, maxSize, growSpeed* Time.deltaTime);

        if (canMove)
        {
            var closestEnemy = findClosestEnemy(transform, detectEnemyDistance);

            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    closestEnemy.position,
                    moveSpeed * Time.deltaTime
                );

                if (Vector2.Distance(transform.position, closestEnemy.position) < crystalExplodeDistance)
                    CrystalExitTimeOver();
            }
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy.transform.position.x <= transform.position.x)
                    enemy.Damage((int)enemy.left.x);
                else if (enemy.transform.position.x > transform.position.x)
                    enemy.Damage((int)enemy.right.x);
            }
        }
    }

    public void CrystalExitTimeOver()
    {
        if (canExplode)
        {
            canMove = false;
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    private void SelfDestroy() => Destroy(gameObject);
}
