using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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

        entity.SetVelocity(xInput * entity.moveSpeed, rb.velocity.y);

        if (xInput == 0 || entity.IsWallDetected())
            fsm.SwitchState(entity.idleState);
    }
}
