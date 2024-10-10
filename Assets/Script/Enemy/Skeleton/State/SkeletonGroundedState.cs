using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : SkeletonState
{

    protected Transform player;

    public SkeletonGroundedState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName, _enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        player = PlayerManger.instance.player.transform;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position , player.position) < enemy.minDetectedDistance) 
            fsm.SwitchState(enemy.battleState);
    }
}
