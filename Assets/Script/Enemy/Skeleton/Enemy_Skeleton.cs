using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    public IState idleState {  get; private set; }
    public IState moveState { get; private set; }
    public IState battleState { get; private set; }
    public IState attackState { get; private set; }
    public IState hitState { get; private set; }
    public IState stunState { get; private set; }

    [Header("Move Info")]
    public float speedMutipulier = 1.5f;    //发现玩家后加速倍率

    [Header("Skeleton Attack Info")]
    [HideInInspector] public float lastTimeAttacked;
    public float minDetectedDistance = 2f;

    [Header("Hit info")]
    public float hitDuration = 1f;

    protected override void Start()
    {
        base.Start();
        idleState = new SkeletonIdleState(this, fsm, "Idle", this);
        moveState = new SkeletonMoveState(this, fsm, "Move", this);
        battleState = new SkeletonBattleState(this, fsm, "Move", this);
        attackState = new SkeletonAttackState(this, fsm, "Attack", this);
        hitState = new SkeletonHitState(this, fsm, "Hit", this);
        stunState = new SkeletonStunState(this, fsm, "Stun", this);
        fsm.SwitchState(idleState);
    }

    protected override void Update()
    {
        base.Update();

        fsm.currentState.Update();
    }

    public override void Damage(int attackDir)
    {
        base.Damage(attackDir);

        fsm.SwitchState(hitState);
    }

    public override bool CanBeStun()
    {
        if(base.CanBeStun())
        {
            fsm.SwitchState(stunState);

            return true;
        }

        return false;
    }

    public void CloseCounterImage()
    {
        counterImage.SetActive(false);
    }

}
