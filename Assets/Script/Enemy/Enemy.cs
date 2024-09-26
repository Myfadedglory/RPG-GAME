using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    Enemy_Skeleton enemy;

    [Header("Detected info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float playerDetectedDistance;

    [Header("Attack info")]
    public float attackDistance;
    public float hatredDistance = 15f;  //³ðºÞ¾àÀë

    [Header("Stun info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStun;
    [SerializeField] protected GameObject counterImage;

    public EnemyStateMachine stateMachine { get; protected set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStun = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStun = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStun()
    {
        if (canBeStun)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public override void Damage(int attackDir)
    {
        base.Damage(attackDir);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(wallCheck.position, Vector2.right * facingDir, playerDetectedDistance, whatIsPlayer | whatIsGround);

        foreach (var hit in hits)
        {
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & whatIsGround) != 0)
            {
                return new RaycastHit2D();
            }

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & whatIsPlayer) != 0)
            {
                return hit;
            }
        }

        return new RaycastHit2D();
    }




    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
