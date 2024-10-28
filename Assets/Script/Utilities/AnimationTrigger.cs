using UnityEngine;

namespace Script.Utilities
{
    public class AnimationTriggers<T> : MonoBehaviour where T : Entity
    {
        protected T entity;

        protected virtual void Start()
        {
            entity = GetComponentInParent<T>();
            if (!entity) Debug.LogError($"Ϊ {typeof(T)} ");
        }

        protected virtual void AnimationTrigger()
        {
            entity.Fsm.CurrentState?.AnimationFinishTrigger();
        }

        protected virtual void AttackTriggerLogic(int attackedDir)
        {
            var colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

            foreach (var hit in colliders)
            {
                if (typeof(T) == typeof(Player.Player) && hit.GetComponent<Enemy.Enemy>() is { } enemy)
                {
                    enemy.Damage(entity.Stats, attackedDir);
                }
                else if (typeof(T) == typeof(Enemy.Enemy) && hit.GetComponent<Player.Player>() is { } player)
                {
                    player.Damage(entity.Stats, attackedDir);
                }
            }
        }

        protected virtual void AttackTrigger()
        {
            AttackTriggerLogic(entity.FacingDir);
        }
    }
}
