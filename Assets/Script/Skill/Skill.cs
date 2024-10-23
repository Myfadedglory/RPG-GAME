using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected LayerMask whatIsEnemy;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManger.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();

            cooldownTimer = cooldown;

            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform ChooseClosestEnemy(Transform detectTransform, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectTransform.position, radius);

        Transform closestEnemy = null;
        
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(detectTransform.position, hit.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    protected virtual Transform ChooseRandomEnemy(Transform detectTransform, float detectDistance)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectDistance, whatIsEnemy);

        if (colliders.Length > 0)
        {
            var randomTarget = colliders[Random.Range(0, colliders.Length)];

            return randomTarget.transform;
        }

        return null;
    }
}
