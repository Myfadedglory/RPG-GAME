using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy entity, FSM fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.FacingDir, rb.velocity.y , enemy.needFlip);

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.Flip();

            fsm.SwitchState(enemy.IdleState);
        }
    }
}
