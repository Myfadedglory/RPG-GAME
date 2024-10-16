using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;

    public PlayerAttackState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = .1f;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + entity.comboWindow)
            comboCounter = 0;

        entity.SetVelocity(
            entity.attackMoveMent[comboCounter].x * entity.FacingDir, 
            entity.attackMoveMent[comboCounter].y , entity.needFlip
        );

        anim.SetInteger("ComboCounter", comboCounter);

        anim.speed = entity.attackSpeed;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        BusyFor(0.15f);

        anim.speed = 1;

        lastTimeAttacked = Time.time;

        comboCounter++;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            entity.SetZeroVelocity();

        if (isAnimationFinished)
            fsm.SwitchState(entity.IdleState);        
    }
}
