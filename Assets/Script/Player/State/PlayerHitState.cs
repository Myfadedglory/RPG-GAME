using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    public PlayerHitState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = entity.hitDuration;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
        BusyFor(0.2f);
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinished) 
            fsm.SwitchState(entity.IdleState);
    }
}
