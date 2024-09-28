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

    #region State

    public IState idleState { get; private set; }
    public IState moveState { get; private set; }
    public IState jumpState { get; private set; }
    public IState airState { get; private set; }
    public IState dashState { get; private set; }
    public IState wallSlide { get; private set; }
    public IState wallJump { get; private set; }
    public IState primaryAttack { get; private set; }
    public IState hitState { get; private set; }
    public IState counterAttack { get; private set; }

    #endregion

    protected override void Start()
    {
        base.Start();
        idleState = new PlayerIdleState(this, fsm, "Idle");
        moveState = new PlayerMoveState(this, fsm, "Move");
        jumpState = new PlayerJumpState(this, fsm, "Jump");
        airState = new PlayerAirState(this, fsm, "Jump");
        dashState = new PlayerDashState(this, fsm, "Dash");
        wallSlide = new PlayerWallSlideState(this, fsm, "WallSlide");
        wallJump = new PlayerWallJumpState(this, fsm, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, fsm, "Attack");
        hitState = new PlayerHitState(this, fsm, "Hit");
        counterAttack = new PlayerCounterAttackState(this, fsm, "Counter");
        fsm.SwitchState(idleState);
    }

    protected override void Update()
    {
        base.Update();
        fsm.currentState.Update();

        dashTimer -= Time.deltaTime;

        CheckDashInput();
    }

    public override void Damage(int attackedDir)
    {
        base.Damage(attackedDir);
        fsm.SwitchState(hitState);
    }

    public void AnimationTrigger() => fsm.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if(IsWallDetected()) 
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManger.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            fsm.SwitchState(dashState);
        }
    }
}
