using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    public PlayerHitState(Player player, FSM _fsm, string _animBoolName) : base(player, _fsm, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.hitDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled) 
            fsm.SwitchState(player.idleState);
    }
}
