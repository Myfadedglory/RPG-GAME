using Script.Player;
using UnityEngine;

namespace Script.Skill
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float cooldown;
        [SerializeField] protected LayerMask whatIsEnemy;
        protected float CooldownTimer;

        protected Player.Player player;

        protected virtual void Start()
        {
            player = PlayerManger.instance.player;
        }

        protected virtual void Update()
        {
            CooldownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if (!(CooldownTimer < 0)) return false;
            
            UseSkill();

            CooldownTimer = cooldown;

            return true;
        }

        protected virtual void UseSkill()
        {

        }

        protected virtual Transform ChooseClosestEnemy(Transform detectTransform, float radius)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(detectTransform.position, radius);

            Transform closestEnemy = null;
        
            float closestDistance = Mathf.Infinity;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy.Enemy>() != null)
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
            var colliders = Physics2D.OverlapCircleAll(transform.position, detectDistance, whatIsEnemy);

            if (colliders.Length <= 0) return null;
            
            var randomTarget = colliders[Random.Range(0, colliders.Length)];

            return randomTarget.transform;

        }
    }
}
