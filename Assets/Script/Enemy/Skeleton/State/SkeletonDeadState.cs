using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : SkeletonState
{
    public SkeletonDeadState(Enemy entity, FSM fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
    }
}
