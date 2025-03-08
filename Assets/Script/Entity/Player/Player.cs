using System.Collections;
using Script.Entity.Player.State;
using Script.Skill;
using Script.Stats;
using Script.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Entity.Player
{
    public class Player : Entity
    {
        
        [HideInInspector] public float moveSpeed;
        [HideInInspector] public Vector2 jumpForce;
        [HideInInspector] public float dashSpeed;
        public float DashDir { get; private set; }
        public PlayerConfig playerConfig;
        public GameObject totalMenu;

        public SkillManager Skill {  get; private set; }
        public GameObject Sword { get; private set; }

        #region Mutiplier info

        public float airMoveMultiplier = .8f;
        public float wallSlideMultiplier = .7f;
        public float wallJumpMultiplier = 1.1f;

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
            
            moveSpeed = playerConfig.defaultMoveSpeed;
            jumpForce = playerConfig.defaultJumpForce;
            dashSpeed = playerConfig.defaultDashSpeed;

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

            Skill = SkillManager.instance; 

            Fsm.SwitchState(IdleState);
        }

        protected override void Update()
        {
            base.Update();

            Fsm.CurrentState.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                totalMenu.GetComponent<UI.UI>()?.skillTooltip.HideTooltip();
                totalMenu.GetComponent<UI.UI>()?.tooltip.HideTooltip();
                totalMenu.GetComponent<UI.UI>()?.craftTooltip.HideTooltip();
                totalMenu.GetComponent<UI.UI>()?.ShowOrHideUI();
            }
            
            if(totalMenu.GetComponent<UI.UI>().UIOpenStatus()) return;
            
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
            
                moveSpeed = playerConfig.defaultMoveSpeed;
                jumpForce = playerConfig.defaultJumpForce;
                dashSpeed = playerConfig.defaultDashSpeed;
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

        public override void Damage(CharacterStats from, Vector2 attackedDir)
        {
            Fsm.SwitchState(HitState);

            base.Damage(from, attackedDir);
        }

        public void AnimationTrigger() => Fsm.CurrentState.AnimationFinishTrigger();

        private void CheckDashInput()
        {
            if (IsWallDetected()||!Input.GetKeyDown(KeyCode.LeftShift) || !SkillManager.instance.Dash.CanUseSkill()) return;
            
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
