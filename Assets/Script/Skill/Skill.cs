using Script.Entity.Player;
using UnityEngine;

namespace Script.Skill
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float cooldown;
        [SerializeField] protected LayerMask whatIsEnemy;
        protected float CooldownTimer;

        protected Player Player;

        protected virtual void Start()
        {
            Player = PlayerManager.instance.player;
        }

        protected virtual void Update()
        {
            CooldownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if (!(CooldownTimer <= 0)) return false;
            
            UseSkill();

            CooldownTimer = cooldown;

            return true;
        }

        protected virtual void UseSkill()
        {

        }

        protected virtual Transform ChooseClosestEnemy(Transform detectTransform, float radius)
        {
            var colliders = Physics2D.OverlapCircleAll(detectTransform.position, radius);

            Transform closestEnemy = null;
        
            var closestDistance = Mathf.Infinity;

            foreach (var hit in colliders)
            {
                if (!hit.GetComponent<Entity.Enemy.Enemy>()) continue;
                
                var distance = Vector2.Distance(detectTransform.position, hit.transform.position);

                if (distance >= closestDistance) continue;
                
                closestDistance = distance;
                closestEnemy = hit.transform;
            }

            return closestEnemy;
        }

        protected virtual Transform ChooseRandomEnemy(Transform detectTransform, float detectDistance)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, detectDistance, whatIsEnemy);

            if (colliders.Length <= 0) return null;
            
            var randomTarget = colliders[Random.Range(0, colliders.Length)];

            return randomTarget.transform;
        }
    }
}
