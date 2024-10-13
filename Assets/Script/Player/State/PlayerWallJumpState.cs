using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = .4f;

        entity.SetVelocity(entity.horizonJumpForce * - entity.facingDir / entity.wallJumpMutiplier, entity.verticalJumpForce * entity.wallJumpMutiplier ,entity.needFlip);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            fsm.SwitchState(entity.airState);
    }
}
