using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, FSM _fsm, string _animBoolName) : base(player, _fsm, _animBoolName)
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
        entity.SetXZeroVelocity();
        if(Input.GetKeyUp(KeyCode.Mouse1)) 
            fsm.SwitchState(entity.idleState);
    }
}
