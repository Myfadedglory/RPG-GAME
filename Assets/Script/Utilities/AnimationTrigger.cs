using UnityEngine;

public class AnimationTriggers<T> : MonoBehaviour where T : Entity
{
    protected T entity;

    protected virtual void Start()
    {
        entity = GetComponentInParent<T>();
        if (!entity) Debug.LogError($"������δ�ҵ���Ϊ {typeof(T)} �����");
    }

    protected virtual void AnimationTrigger()
    {
        entity.Fsm.currentState?.AnimationFinishTrigger();
    }

    protected virtual void AttackTriggerLogic(int attackedDir)
    {
        var colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

        foreach (var hit in colliders)
        {
            if (typeof(T) == typeof(Player) && hit.GetComponent<Enemy>() is Enemy enemy)
            {
                enemy.Damage(entity.Stats, attackedDir);
            }
            else if (typeof(T) == typeof(Enemy) && hit.GetComponent<Player>() is Player player)
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
