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
    private float growSpeed = 5;
    private float detectEnemyDistance = 20;

    public void SetUpCrystal(float crystalDuration ,bool canExplode, bool canMove, float moveSpeed)
    {
        crystalExitTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
    }

    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;

        if (crystalExitTimer < 0)
            CrystalExitTimeOver();

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed* Time.deltaTime);

        if (canMove)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectEnemyDistance);

            List<Transform> enemyTarget = new List<Transform>();

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                    enemyTarget.Add(hit.transform);
            }

            float closestDistance = Mathf.Infinity;

            int closestIndex = -1;
            int targetIndex = 0;

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

            transform.position = Vector2.MoveTowards(
                transform.position, 
                enemyTarget[targetIndex].position, 
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.Damage();
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
