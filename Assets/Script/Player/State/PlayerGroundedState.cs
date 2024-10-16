using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
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

        if(isBusy) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            fsm.SwitchState(entity.BlackHole);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            fsm.SwitchState(entity.AimSword);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            fsm.SwitchState(entity.CounterAttack);
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            fsm.SwitchState(entity.AttackState);
            return;
        }

        if (!entity.IsGroundDetected())
        {
            fsm.SwitchState(entity.AirState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && entity.IsGroundDetected())
        {
            fsm.SwitchState(entity.JumpState);
            return;
        }
    }

    private bool HasNoSword()
    {
        if(!entity.Sword)
            return true;

        entity.Sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

        return false;
    }
}
