using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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
            fsm.SwitchState(entity.wallJump);
            return;
        }

        if (xInput != 0 && entity.facingDir != xInput)
            fsm.SwitchState(entity.idleState);

        if (yInput < 0)
            entity.SetVelocity(0, rb.velocity.y);
        else
            entity.SetVelocity(0, entity.wallSlideMutiplier * rb.velocity.y);

        if (entity.IsGroundDetected())
            fsm.SwitchState(entity.idleState);
    }
}
