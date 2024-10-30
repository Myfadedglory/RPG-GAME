using System.Collections;
using Script.Utilities;
using UnityEngine;

namespace Script
{
    public abstract class Entity : MonoBehaviour
    {
        [Header("Collision info")]
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected float groundCheckDistance;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected Transform wallCheck;
        [SerializeField] protected float wallCheckDistance;

        [Header("Knock back info")]
        [SerializeField] protected Vector2 knockbackDirection;
        [SerializeField] protected float knockbackDuration = 0.07f;
        private bool isKnocked;

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
        public Fsm Fsm { get; private set; }
        public SpriteRenderer Sr { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CapsuleCollider2D Cd { get; private set; }

        #endregion

        public int FacingDir { get; private set; } = 1;
        private bool facingRight = true;

        protected virtual void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            Fx = GetComponent<EntityFX>();
            Fsm = new Fsm();
            Sr = GetComponentInChildren<SpriteRenderer>();
            Stats = GetComponent<CharacterStats>();
            Cd = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void Update()
        {
            Fsm.CurrentState?.Update();
        }

        public virtual void Damage(CharacterStats from, int attackDir, bool isMagic)
        {
            Fx.StartCoroutine("FlashFX");

            StartCoroutine(nameof(HitKnockback), attackDir);            

            from.DoDamage(Stats, isMagic);
        }

        public virtual void Damage(CharacterStats from, bool isMagic)
        {
            Fx.StartCoroutine("FlashFX");

            StartCoroutine(nameof(HitKnockback), -FacingDir);

            from.DoDamage(Stats, isMagic);
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

        protected virtual void FlipController(float x)
        {
            switch (Rb.velocity.x)
            {
                case > 0 when !facingRight:
                case < 0 when facingRight:
                    Flip();
                    break;
            }
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

        public void MakeTransparent(bool transparent)
        {
            Sr.color = transparent ? Color.clear : Color.white;
        }
        
        public abstract void Die();

    }
}
