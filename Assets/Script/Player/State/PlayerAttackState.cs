using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerAttackState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = .1f;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        entity.SetVelocity(entity.attackMoveMent[comboCounter].x * entity.facingDir, entity.attackMoveMent[comboCounter].y , entity.needFlip);

        entity.anim.SetInteger("ComboCounter", comboCounter);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        comboCounter++;

        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            entity.SetZeroVelocity();

        if (isAnimationFinished && !Input.GetKey(KeyCode.Mouse0))
            fsm.SwitchState(entity.idleState);
        
        if (! isAnimationFinished && Input.GetKey(KeyCode.Mouse0))
            return;
    }
}
