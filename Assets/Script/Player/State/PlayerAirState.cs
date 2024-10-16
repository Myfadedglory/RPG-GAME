using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        entity.SetXZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if(entity.IsWallDetected())
            fsm.SwitchState(entity.WallSlide);

        if(xInput != 0)
            entity.SetVelocity(entity.airMoveMutiplier * xInput * entity.moveSpeed , rb.velocity.y , entity.needFlip);

        if(entity.IsGroundDetected())
            fsm.SwitchState(entity.IdleState);
    }
}
