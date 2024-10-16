using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Detected info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float playerDetectedDistance;

    [Header("Attack info")]
    public float battleTime = 6f;
    public float attackDistance = 1.5f;
    public float hatredDistance = 15f;  //³ðºÞ¾àÀë
    public float attackCoolDown = 1f;

    [Header("Stun info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStun;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed = 2.0f;
    public float idleTime = 10f;
    private float defaultMoveSpeed = 2.0f;

    protected override void Start()
    {
        base.Start();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        fsm.currentState.Update();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;

            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;

            anim.speed = 1;
        }
    }

    protected virtual IEnumerable FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window

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

    #endregion

    public virtual bool CanBeStun()
    {
        if (canBeStun)
        {
            CloseCounterAttackWindow();

            return true;
        }

        return false;
    }

    public void AnimationTrigger() => fsm.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(wallCheck.position, Vector2.right * FacingDir, playerDetectedDistance, whatIsPlayer | whatIsGround);

        foreach (var hit in hits)
        {
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & whatIsGround) != 0)
                return new RaycastHit2D();

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & whatIsPlayer) != 0)
                return hit;
        }

        return new RaycastHit2D();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
    }
}
