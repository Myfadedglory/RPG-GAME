using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack info")]
    public Vector2[] attackMoveMent;
    public float attackSpeed = 1f;
    public float comboWindow = 1f;

    [Header("Move info")]
    public float moveSpeed = 3.80f;
    public float horizonJumpForce = 8f;
    public float verticalJumpForce = 10f;

    [Header("Dash info")]
    [SerializeField] public float dashCoolDown = 1.2f;
    public float dashSpeed = 25;
    public float dashDuration = .2f;
    public float dashDir { get; private set; }

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

        Fsm.currentState.Update();

        CheckDashInput();

        if (Input.GetKeyDown(KeyCode.F))
            Skill.crystal.CanUseSkill();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        Sword = _newSword;
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

    public void AnimationTrigger() => Fsm.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if(IsWallDetected()) 
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManger.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = FacingDir;

            Fsm.SwitchState(DashState);
        }
    }

    public override void Die()
    {
        Fsm.SwitchState(DeadState);
    }
}
