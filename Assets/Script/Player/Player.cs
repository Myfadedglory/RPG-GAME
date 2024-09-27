using UnityEngine;

public class Player : Entity
{
    [Header("Attack info")]
    public Vector2[] attackMoveMent;

    [Header("Move info")]
    public float moveSpeed = 3.80f;
    public float horizonJumpForce = 8f;
    public float verticalJumpForce = 10f;

    [Header("Dash info")]
    [SerializeField] public float dashCoolDown = 1.2f;
    public float dashSpeed = 25;
    public float dashDuration = .2f;
    private float dashTimer = 0;
    public float dashDir { get; private set; }

    [Header("Hit info")]
    public float hitDuration = 0.2f;

    [Header("Counter Attack info")]
    public float counterAttackDuration;

    #region Mutiplier info

    public float airMoveMutiplier = .8f;
    public float wallSlideMutiplier = .7f;
    public float wallJumpMutiplier = 1.1f;

    #endregion

    public PlayerStateMachine stateMachine { get; private set; }

    #region State

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerHitState hitState { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        hitState = new PlayerHitState(this, stateMachine, "Hit");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "Counter");
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

        dashTimer -= Time.deltaTime;

        CheckDashInput();
    }

    public override void Damage(int attackedDir)
    {
        base.Damage(attackedDir);
        stateMachine.ChangeState(hitState);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if(IsWallDetected()) 
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManger.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
}
