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

        entity.Skill.clone.CreateCloneOnDashStart();

        stateTimer = entity.dashDuration;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        entity.Skill.clone.CreateCloneOnDashOver();

        entity.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if(!entity.IsGroundDetected() && entity.IsWallDetected())
            fsm.SwitchState(entity.WallSlide);

        entity.SetVelocity(entity.dashSpeed * entity.dashDir , 0 , entity.needFlip);

        if (stateTimer < 0) 
            fsm.SwitchState(entity.IdleState);
    }
}
