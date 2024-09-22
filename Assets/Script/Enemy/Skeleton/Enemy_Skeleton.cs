using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    public SkeletonIdleState skeletonIdleState {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        skeletonIdleState = new SkeletonIdleState(this , stateMachine , "Idle");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(skeletonIdleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
}
