using UnityEngine;

namespace Script.Utilities
{
    public class AnimationTriggers<T> : MonoBehaviour where T : Entity
    {
        protected T Entity;

        protected virtual void Start()
        {
            Entity = GetComponentInParent<T>();
            if (!Entity) Debug.LogError($"Ϊ {typeof(T)} ");
        }

        protected virtual void AnimationTrigger()
        {
            Entity.Fsm.CurrentState?.AnimationFinishTrigger();
        }

        protected virtual void AttackTriggerLogic(int attackedDir)
        {
            var colliders = Physics2D.OverlapCircleAll(Entity.attackCheck.position, Entity.attackCheckDistance);

            foreach (var hit in colliders)
            {
                if (typeof(T) == typeof(Player.Player) && hit.GetComponent<Enemy.Enemy>() is { } enemy)
                {
                    enemy.Damage(Entity.Stats, attackedDir);
                }
                else if (typeof(T) == typeof(Enemy.Enemy) && hit.GetComponent<Player.Player>() is { } player)
                {
                    player.Damage(Entity.Stats, attackedDir);
                }
            }
        }

        protected virtual void AttackTrigger()
        {
            AttackTriggerLogic(Entity.FacingDir);
        }
    }
}
