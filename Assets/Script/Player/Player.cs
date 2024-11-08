using System.Collections;
using Script.Player.State;
using Script.Stats;
using Script.Utilities;
using UnityEngine;

namespace Script.Player
{
    public class Player : Entity
    {
        [Header("Attack info")]
        public Vector2[] attackMoveMent;
        public float attackSpeed = 1f;
        public float comboWindow = 1f;

        [Header("Move info")]
        [HideInInspector] public float moveSpeed;
        [HideInInspector] public Vector2 jumpForce;
        public float defaultMoveSpeed = 3.80f;
        public Vector2 defaultJumpForce = new (4, 8);

        [Header("Dash info")]
        [HideInInspector] public float dashSpeed;
        public float defaultDashSpeed = 40;
        public float dashDuration = .2f;
        public float DashDir { get; private set; }

        [Header("Hit info")]
        public float hitDuration = 0.2f;

        [Header("Counter Attack info")]
        public float counterAttackDuration = 0.1f;
        public float swordReturnForce = 7f;

        public SkillManger Skill {  get; private set; }
        public GameObject Sword { get; private set; }

        #region Mutiplier info

        public float airMoveMutiplier = .8f;
        public float wallSlideMutiplier = .7f;
        public float wallJumpMutiplier = 1.1f;

        #endregion

        #region State

        public IState IdleState { get; private set; }
        public IState MoveState { get; private set; }
        public IState JumpState { get; private set; }
        public IState AirState { get; private set; }
        public IState DeadState { get; private set; }
        public IState DashState { get; private set; }
        public IState WallSlide { get; private set; }
        public IState WallJump { get; private set; }
        public IState AttackState { get; private set; }
        public IState HitState { get; private set; }
        public IState CounterAttack { get; private set; }
        public IState AimSword { get; private set; }
        public IState CatchSword { get; private set; }
        public IState BlackHole { get; private set; }

        #endregion

        protected override void Start()
        {
            base.Start();
            
            moveSpeed = defaultMoveSpeed;
            jumpForce = defaultJumpForce;
            dashSpeed = defaultDashSpeed;

            IdleState = new PlayerIdleState(this, Fsm, "Idle");
            MoveState = new PlayerMoveState(this, Fsm, "Move");
            JumpState = new PlayerJumpState(this, Fsm, "Jump");
            AirState = new PlayerAirState(this, Fsm, "Jump");
            DeadState = new PlayerDeadState(this, Fsm, "Dead");
            DashState = new PlayerDashState(this, Fsm, "Dash");
            WallSlide = new PlayerWallSlideState(this, Fsm, "WallSlide");
            WallJump = new PlayerWallJumpState(this, Fsm, "Jump");
            AttackState = new PlayerAttackState(this, Fsm, "Attack");
            HitState = new PlayerHitState(this, Fsm, "Hit");
            CounterAttack = new PlayerCounterAttackState(this, Fsm, "Counter");
            AimSword = new PlayerAimSwordState(this, Fsm, "AimSword");
            CatchSword = new PlayerCatchSwordState(this, Fsm, "CatchSword");
            BlackHole = new PlayerBlackholeState(this, Fsm, "Jump");

            Skill = SkillManger.instance; 

            Fsm.SwitchState(IdleState);
        }

        protected override void Update()
        {
            base.Update();

            Fsm.CurrentState.Update();

            CheckDashInput();

            if (Input.GetKeyDown(KeyCode.F))
                Skill.Crystal.CanUseSkill();
        }

        public override void SlowEntityFor(float percentage, float duration)
        {
            base.SlowEntityFor(percentage, duration);
            
            StartCoroutine(SlowEntity());
            return;

            IEnumerator SlowEntity()
            {
                moveSpeed *= 1-percentage;
                jumpForce *= 1-percentage;
                dashSpeed *= 1-percentage;
                Anim.speed *= 1-percentage;
            
                yield return new WaitForSeconds(duration);
            
                moveSpeed = defaultMoveSpeed;
                jumpForce = defaultJumpForce;
                dashSpeed = defaultDashSpeed;
                Anim.speed = 1;
            }
        }
        
        
        public void AssignNewSword(GameObject newSword)
        {
            Sword = newSword;
        }

        public void CatchTheSword()
        {
            Fsm.SwitchState(CatchSword);

            Destroy(Sword);
        }

        public override void Damage(CharacterStats from, int attackedDir)
        {
            Fsm.SwitchState(HitState);

            base.Damage(from, attackedDir);
        }

        public void AnimationTrigger() => Fsm.CurrentState.AnimationFinishTrigger();

        private void CheckDashInput()
        {
            if(IsWallDetected()) 
                return;

            if (!Input.GetKeyDown(KeyCode.LeftShift) || !SkillManger.instance.Dash.CanUseSkill()) return;
            
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir == 0)
                DashDir = FacingDir;

            Fsm.SwitchState(DashState);
        }

        public override void Die()
        {
            Fsm.SwitchState(DeadState);
        }
    }
}
