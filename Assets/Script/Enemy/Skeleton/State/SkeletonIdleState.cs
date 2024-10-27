using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy entity, FSM fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = enemy.idleTime;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0) 
            fsm.SwitchState(enemy.MoveState);
    }
}
