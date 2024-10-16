using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : SkeletonState
{
    public SkeletonStunState(Enemy entity, FSM fsm, string animBoolName, Enemy_Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.FacingDir * enemy.stunDirection.x, enemy.stunDirection.y);
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
