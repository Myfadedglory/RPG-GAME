using System;
using Script.Config;
using Script.Entity.Player;
using Script.Stats;
using UnityEngine;

namespace Script.Skill.Crystal
{
    public class CrystalSkillController : MonoBehaviour
    {
        private static readonly int Explode = Animator.StringToHash("Explode");
        private Animator Anim => GetComponent<Animator>();
        private CircleCollider2D Cd => GetComponent<CircleCollider2D>();

        private float crystalExitTimer;
        
        private CrystalConfig config;
        private bool canMove;
        private bool canGrow;
        
        private Transform followTarget;

        public void SetUpCrystal(
            CrystalConfig crystalConfig,
            bool chooseRandomTarget,
            Func<Transform, float, Transform> chooseClosestEnemy,
            Func<Transform, float, Transform> chooseRandomEnemy
        )
        {
            config = crystalConfig;
            canMove = crystalConfig.crystalMove.GetSkillCondition();
            crystalExitTimer = crystalConfig.duration;
   
            followTarget = chooseRandomTarget ? 
                chooseRandomEnemy(transform, crystalConfig.detectDistance) : 
                chooseClosestEnemy(transform, crystalConfig.detectDistance);
        }

        private void Update()
        {
            crystalExitTimer -= Time.deltaTime;

            if (crystalExitTimer < 0)
                CrystalExitTimeOver();

            if (canGrow)
                transform.localScale = Vector2.Lerp(transform.localScale, config.maxSize, config.growSpeed * Time.deltaTime);
            
            if (!canMove || !followTarget) return;
            
            transform.position = Vector2.MoveTowards(
                transform.position,
                followTarget.position,
                config.moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, followTarget.position) < config.explodeDistance)
                CrystalExitTimeOver();
        }

        private void AnimationExplodeEvent()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, Cd.radius);

            foreach(var hit in colliders)
            {
                if (hit.GetComponent<Entity.Enemy.Enemy>() == null) continue;
                
                var enemy = hit.GetComponent<Entity.Enemy.Enemy>();
                if (enemy.transform.position.x <= transform.position.x)
                    enemy.MagicDamage(PlayerManager.instance.player.Stats, enemy.Left, MagicType.Lightning);
                else if (enemy.transform.position.x > transform.position.x)
                    enemy.MagicDamage(PlayerManager.instance.player.Stats, enemy.Right, MagicType.Lightning);
            }
        }

        public void CrystalExitTimeOver()
        {
            if (config.crystalExplode.GetSkillCondition())
            {
                canMove = false;
                canGrow = true;
                Anim.SetTrigger(Explode);
            }
            else
                SelfDestroy();
        }

        private void SelfDestroy() => Destroy(gameObject);
    }
}
