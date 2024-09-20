using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move info")]
    public float moveSpeed = 3.80f;
    public float horizonJumpForce = 8f;
    public float verticalJumpForce = 10f;

    [Header("Dash info")]
    [SerializeField] public float dashCoolDown = 1.2f;
    public float dashSpeed = 25;
    public float dashDuration = .2f;
    private float dashTimer = 0;
    public float dashDir {  get; private set; }

    [Header("Collision Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;


    #region Component

    public Animator anim {get ;private set;}
    public Rigidbody2D rb { get ; private set;}

    #endregion

    public PlayerStateMachine stateMachine {  get; private set; }


    #region State

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    #endregion
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this , stateMachine , "Idle");
        moveState = new PlayerMoveState(this , stateMachine , "Move");
        jumpState = new PlayerJumpState(this , stateMachine , "Jump");
        airState = new PlayerAirState(this , stateMachine , "Jump");
        dashState = new PlayerDashState(this , stateMachine , "Dash");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        dashTimer -= Time.deltaTime;

        CheckDashInput();
    }

    private void CheckDashInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            dashTimer = dashCoolDown;
            stateMachine.ChangeState(dashState);
        }
    }


    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,Vector2.down , groundCheckDistance , whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance , whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    #region Flip

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if(rb.velocity.x < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity

    public void SetXZeroVerlocity() => rb.velocity = new Vector2(0,rb.velocity.y);

    public void SetYZeroVerlocity() => rb.velocity = new Vector2(rb.velocity.x,0);

    public void SetZeroVerlocity() => rb.velocity = new Vector2 (0, 0);

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion
}
