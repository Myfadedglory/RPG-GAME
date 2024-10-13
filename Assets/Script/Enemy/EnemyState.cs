using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState<Enemy>
{

    public EnemyState(Enemy entity, FSM fsm, string animBoolName) : base(entity, fsm, animBoolName)
    {
    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
    }
}
