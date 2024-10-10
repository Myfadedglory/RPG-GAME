using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : AnimationTriggers<Player> 
{
    private Player player => GetComponentInParent<Player>();

    protected override void AttackTriggerLogic(int facingDir)
    {
        base.AttackTriggerLogic(facingDir);
    }

    protected override void AttackTrigger()
    {
        base.AnimationTrigger();

        AttackTriggerLogic(player.facingDir);
    }

    private void ThrowSword()
    {
        SkillManger.instance.sword.CreateSword();
    }
}
