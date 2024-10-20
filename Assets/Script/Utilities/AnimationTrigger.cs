using UnityEngine;

public class AnimationTriggers<T> : MonoBehaviour where T : Entity
{
    protected T entity;

    private void Start()
    {
        entity = GetComponentInParent<T>();
        if (!entity) Debug.LogError($"������δ�ҵ���Ϊ {typeof(T)} �����");
    }

    protected virtual void AnimationTrigger()
    {
        entity.fsm.currentState?.AnimationFinishTrigger();
    }

    protected virtual void AttackTriggerLogic(int attackedDir)
    {
    }

    protected virtual void AttackTrigger()
    {
        AttackTriggerLogic(entity.FacingDir);
    }
}
