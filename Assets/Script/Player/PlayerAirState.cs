using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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
            fsm.SwitchState(entity.wallSlide);

        if(xInput != 0)
            entity.SetVelocity(entity.airMoveMutiplier * xInput * entity.moveSpeed , rb.velocity.y);

        if(entity.IsGroundDetected())
            fsm.SwitchState(entity.idleState);
    }
}
