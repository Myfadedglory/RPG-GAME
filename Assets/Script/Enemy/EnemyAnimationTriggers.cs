using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : AnimationTriggers<Enemy>
{
    protected void OpenCounterWindow() => entity.OpenCounterAttackWindow();

    protected void CloseCounterWindow() => entity.CloseCounterAttackWindow();
}
