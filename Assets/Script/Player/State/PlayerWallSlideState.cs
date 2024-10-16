using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fsm.SwitchState(entity.WallJump);

            return;
        }

        if (xInput != 0 && entity.FacingDir != xInput)
            fsm.SwitchState(entity.IdleState);

        if (yInput < 0)
            entity.SetVelocity(0, rb.velocity.y , entity.needFlip);
        else
            entity.SetVelocity(0, entity.wallSlideMutiplier * rb.velocity.y, entity.needFlip);

        if (entity.IsGroundDetected())
            fsm.SwitchState(entity.IdleState);
    }
}
