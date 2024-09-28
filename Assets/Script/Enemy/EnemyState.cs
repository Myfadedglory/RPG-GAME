using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState<Enemy>
{
    protected Enemy enemyBase;

    public EnemyState(Enemy entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
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
