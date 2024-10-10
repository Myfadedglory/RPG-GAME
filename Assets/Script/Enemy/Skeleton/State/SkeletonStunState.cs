using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : SkeletonState
{
    public SkeletonStunState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName, _enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        enemy.fx.Invoke("CancelRedColorBlink", 0);
    }

    public override void Update()
    {
        base.Update();

        if( stateTimer < 0 )
            fsm.SwitchState(enemy.idleState);
    }
}
