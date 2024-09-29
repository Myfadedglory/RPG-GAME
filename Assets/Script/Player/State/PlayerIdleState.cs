using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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

        if (!(xInput == 0 || (xInput == entity.facingDir) && entity.IsWallDetected()))
            fsm.SwitchState(entity.moveState);
    }
}
