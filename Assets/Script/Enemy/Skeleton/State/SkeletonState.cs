using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected Enemy_Skeleton enemy;

    public SkeletonState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName)
    {
        this.enemy = _enemy;
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
