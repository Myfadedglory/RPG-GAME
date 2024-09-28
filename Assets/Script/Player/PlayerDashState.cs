using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        stateTimer = entity.dashDuration;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
        entity.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if(!entity.IsGroundDetected() && entity.IsWallDetected())
            fsm.SwitchState(entity.wallSlide);

        entity.SetVelocity(entity.dashSpeed * entity.dashDir , 0);

        if (stateTimer < 0) 
            fsm.SwitchState(entity.idleState);
    }
}
