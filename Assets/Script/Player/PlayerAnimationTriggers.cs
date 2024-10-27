using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : AnimationTriggers<Player> 
{
    private void ThrowSword()
    {
        SkillManger.instance.sword.CreateSword();
    }
}
