using Script.Enemy.Skeleton.State;
using Script.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Enemy.Skeleton
{
    public class Skeleton : Enemy
    {

        public IState IdleState {  get; private set; }
        public IState MoveState { get; private set; }
        public IState BattleState { get; private set; }
        public IState AttackState { get; private set; }
        public IState HitState { get; private set; }
        public IState StunState { get; private set; }
        public IState DeadState { get; private set; }
        
        [Header("Move Info")]
        public float speedMultiplier = 1.5f;    //发现玩家后加速倍率

        [Header("Skeleton Attack Info")]
        [HideInInspector] public float lastTimeAttacked;
        public float minDetectedDistance = 2f;

        [Header("Hit info")]
        public float hitDuration = 1f;

        protected override void Start()
        {
            base.Start();
            IdleState = new SkeletonIdleState(this, Fsm, "Idle", this);
            MoveState = new SkeletonMoveState(this, Fsm, "Move", this);
            BattleState = new SkeletonBattleState(this, Fsm, "Move", this);
            AttackState = new SkeletonAttackState(this, Fsm, "Attack", this);
            HitState = new SkeletonHitState(this, Fsm, "Hit", this);
            StunState = new SkeletonStunState(this, Fsm, "Stun", this);
            DeadState = new SkeletonDeadState(this, Fsm, "Dead", this);
            Fsm.SwitchState(IdleState);
        }

        protected override void Update()
        {
            base.Update();

            Fsm.CurrentState.Update();
        }

        public override void Damage(CharacterStats from, int attackDir, bool isMagic)
        {
            Fsm.SwitchState(HitState);

            base.Damage(from, attackDir, isMagic);
        }

        public override bool CanBeStun()
        {
            if (!base.CanBeStun()) return false;
            
            Fsm.SwitchState(StunState);

            return true;
        }

        public void CloseCounterImage()
        {
            counterImage.SetActive(false);
        }

        public override void Die()
        {
            Fsm.SwitchState(DeadState);
        }
    }
}
