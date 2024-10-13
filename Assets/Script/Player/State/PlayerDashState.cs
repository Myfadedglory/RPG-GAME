using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        entity.skill.clone.CreateClone(entity.transform, new Vector3());

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

        entity.SetVelocity(entity.dashSpeed * entity.dashDir , 0 , entity.needFlip);

        if (stateTimer < 0) 
            fsm.SwitchState(entity.idleState);
    }
}
