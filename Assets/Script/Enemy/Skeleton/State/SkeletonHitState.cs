using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHitState : SkeletonState
{

    private IState previousState;

    public SkeletonHitState(Enemy entity, FSM fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = enemy.hitDuration;

        enemy.CloseCounterAttackWindow();
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinished)
            fsm.SwitchState(enemy.IdleState);
    }
}
