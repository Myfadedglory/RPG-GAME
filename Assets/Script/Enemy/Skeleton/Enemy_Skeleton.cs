using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    public SkeletonIdleState idleState {  get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonHitState hitState { get; private set; }
    public SkeletonStunState stunState { get; private set; }

    [Header("Move Info")]
    public float skeletonMoveSpeed = 2.0f;
    public float idleTime = 1f;
    public float speedMutipulier = 1.5f;    //发现玩家后加速倍率

    [Header("Skeleton Attack Info")]
    public float battleTime = 6f;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttacked;
    public float minDetectedDistance = 2f;

    [Header("Hit info")]
    public float hitDuration = 0.2f;

    [Header("Stun info")]
    public float stunDuration = 1f;
    public Vector2 stunDirection;

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this , stateMachine , "Idle" , this);
        moveState = new SkeletonMoveState(this , stateMachine , "Move" , this);
        battleState = new SkeletonBattleState(this , stateMachine , "Move" , this);
        attackState = new SkeletonAttackState(this , stateMachine , "Attack" , this);
        hitState = new SkeletonHitState(this , stateMachine , "Hit" , this);
        stunState = new SkeletonStunState(this , stateMachine , "Stun" , this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void Damage(int attackDir)
    {
        base.Damage(attackDir);
        stateMachine.ChangeState(hitState);
    }

}
