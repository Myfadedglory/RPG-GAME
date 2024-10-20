using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : AnimationTriggers<Player> 
{
    protected override void AttackTriggerLogic(int attackedDir)
    {
        base.AttackTriggerLogic(attackedDir);
        var colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.Damage(attackedDir);
        }
    }

    private void ThrowSword()
    {
        SkillManger.instance.sword.CreateSword();
    }
}
