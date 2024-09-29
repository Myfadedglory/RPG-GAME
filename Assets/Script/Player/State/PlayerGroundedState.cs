using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Mouse1))
            fsm.SwitchState(entity.aimSword);

        if (Input.GetKeyDown(KeyCode.Q))
            fsm.SwitchState(entity.counterAttack);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            fsm.SwitchState(entity.attackState);
        }

        if (!entity.IsGroundDetected())
            fsm.SwitchState(entity.airState);

        if (Input.GetKeyDown(KeyCode.Space) && entity.IsGroundDetected())
            fsm.SwitchState(entity.jumpState);
    }
}
