using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;

    public Vector2 right = new(1,0);
    public Vector2 left = new(-1,0);
    public Vector2 up = new(0,1);
    public Vector2 down = new(0,-1);

    [Header("Attack info")]
    public Transform attackCheck;
    public float attackCheckDistance;

    public bool needFlip = true;

    #region Component

    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX Fx { get; private set; }
    public FSM Fsm { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    public CharacterStats Stats { get; private set; }
    public CapsuleCollider2D Cd { get; private set; }

    #endregion

    public int FacingDir { get; private set; } = 1;
    protected bool facingRight = true;

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Fx = GetComponent<EntityFX>();
        Fsm = new FSM();
        Sr = GetComponentInChildren<SpriteRenderer>();
        Stats = GetComponent<CharacterStats>();
        Cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
        Fsm.currentState?.Update();
    }

    /*
     * 存在问题: 伤害来源问题
     * 目前初步解决: 基础解决伤害逻辑
     * 后续解决: 考虑将伤害部分组件化，便于维护和升级
     */

    public virtual void Damage(CharacterStats from, int attackDir)
    {
        Fx.StartCoroutine("FlashFX");

        StartCoroutine(nameof(HitKnockback), attackDir);            

        from.DoDamage(Stats);
    }

    public virtual void Damage(CharacterStats from)
    {
        Fx.StartCoroutine("FlashFX");

        StartCoroutine(nameof(HitKnockback), -FacingDir);

        from.DoDamage(Stats);
    }

    protected virtual IEnumerator HitKnockback(int attackDir)
    {
        isKnocked = true;

        Rb.velocity = new Vector2(knockbackDirection.x * attackDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        SetZeroVelocity();

        isKnocked = false;
    }

    #region Collision

    public virtual bool IsGroundDetected() => Physics2D.Raycast(
        groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(
        wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            groundCheck.position, 
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
        
        Gizmos.DrawLine(
            wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * FacingDir, wallCheck.position.y)
        );
        
        Gizmos.DrawWireSphere(
            attackCheck.position, 
            attackCheckDistance
        );
    }

    #endregion

    #region Flip

    public virtual void Flip()
    {
        FacingDir = - FacingDir;

        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (Rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (Rb.velocity.x < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity

    public virtual void SetXZeroVelocity() => Rb.velocity = new Vector2(0, Rb.velocity.y);

    public virtual void SetYZeroVelocity() => Rb.velocity = new Vector2(Rb.velocity.x, 0);

    public virtual void SetZeroVelocity() => Rb.velocity = new Vector2(0, 0);

    public virtual void SetVelocity(float xVelocity, float yVelocity , bool needFlip)
    {
        if(isKnocked)
            return;

        Rb.velocity = new Vector2(xVelocity, yVelocity);

        if(needFlip)
            FlipController(xVelocity);
    }

    #endregion

    public void MakeTransprent(bool transprent)
    {
        if (transprent)
            Sr.color = Color.clear;
        else
            Sr.color = Color.white;
    }


    public abstract void Die();

}
