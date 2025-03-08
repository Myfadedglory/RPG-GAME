using UnityEngine;

namespace Script.Utilities
{
    public class AnimationTriggers<T> : MonoBehaviour where T : Entity.Entity
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
                if (typeof(T) == typeof(Entity.Player.Player) && hit.GetComponent<Entity.Enemy.Enemy>() is { } enemy)
                {
                    enemy.Damage(Entity.Stats, new Vector2(attackedDir, 0));
                }
                else if (typeof(T) == typeof(Entity.Enemy.Enemy) && hit.GetComponent<Entity.Player.Player>() is { } player)
                {
                    player.Damage(Entity.Stats, new Vector2(attackedDir, 0));
                }
            }
        }

        protected virtual void AttackTrigger()
        {
            AttackTriggerLogic(Entity.FacingDir);
        }
    }
}
