using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected Skeleton enemy;

    public SkeletonState(Enemy entity, FSM fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName)
    {
        this.enemy = enemy;
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
