using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHitState : SkeletonState
{

    private IState previousState;

    public SkeletonHitState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName, _enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        stateTimer = enemy.hitDuration;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();
        if (isAnimationFinished)
            fsm.SwitchState(enemy.idleState);
    }
}
