using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : AnimationTriggers<Enemy>
{
    protected override void AttackTriggerLogic(int attackedDir)
    {
        base.AttackTriggerLogic(attackedDir);

        var colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Player>()?.Damage(attackedDir);
        }
    }

    protected void OpenCounterWindow() => entity.OpenCounterAttackWindow();

    protected void CloseCounterWindow() => entity.CloseCounterAttackWindow();
}
