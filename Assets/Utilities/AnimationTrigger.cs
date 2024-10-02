using UnityEngine;

public class AnimationTriggers<T> : MonoBehaviour where T : Entity
{
    protected T entity;

    private void Start()
    {
        entity = GetComponentInParent<T>();
        if (!entity) Debug.LogError($"������δ�ҵ���Ϊ {typeof(T)} �����");
    }

    protected void AnimationTrigger()
    {
        entity.fsm.currentState?.AnimationFinishTrigger();
    }

    protected virtual void AttackTriggerLogic(int attackedDir)
    {
        var colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Entity>() != null)
                hit.GetComponent<Entity>().Damage(attackedDir);
        }
    }

    protected virtual void AttackTrigger()
    {

    }
}
