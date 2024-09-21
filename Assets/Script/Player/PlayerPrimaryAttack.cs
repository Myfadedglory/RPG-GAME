using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;

        if(comboCounter > 2 || Time.time >= lastTimeAttacked +  comboWindow)
            comboCounter = 0;

        player.SetVelocity(player.attackMoveMent[comboCounter].x * player.facingDir, player.attackMoveMent[comboCounter].y);

        player.anim.SetInteger("ComboCounter" , comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;

        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <  0)
            player.SetZeroVerlocity();

        if(triggerCalled) 
            stateMachine.ChangeState(player.idleState);
    }
}
