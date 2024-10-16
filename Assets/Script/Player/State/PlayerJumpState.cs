using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        rb.velocity = new Vector2(rb.velocity.x, entity.verticalJumpForce);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y > 0)
            fsm.SwitchState(entity.AirState);
    }
}
