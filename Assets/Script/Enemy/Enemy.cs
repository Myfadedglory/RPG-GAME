using System.Collections;
using UnityEngine;

namespace Script.Enemy
{
    public abstract class Enemy : Entity
    {
        [Header("Detected info")]
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected float playerDetectedDistance;

        [Header("Attack info")]
        public float battleTime = 6f;
        public float attackDistance = 1.5f;
        public float hatredDistance = 15f;  //��޾���
        public float attackCoolDown = 1f;

        [Header("Stun info")]
        public float stunDuration;
        public Vector2 stunDirection;
        private bool canBeStun;
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

            Fsm.CurrentState.Update();
        }

        public virtual void FreezeTime(bool timeFrozen)
        {
            if (timeFrozen)
            {
                moveSpeed = 0;

                Anim.speed = 0;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;

                Anim.speed = 1;
            }
        }

        protected virtual IEnumerable FreezeTimeFor(float seconds)
        {
            FreezeTime(true);

            yield return new WaitForSeconds(seconds);

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

        public void AnimationTrigger() => Fsm.CurrentState.AnimationFinishTrigger();

        public virtual RaycastHit2D IsPlayerDetected()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(wallCheck.position, Vector2.right * FacingDir, playerDetectedDistance, whatIsPlayer | whatIsGround);

            foreach (var hit in hits)
            {
                if (hit.collider is not null && ((1 << hit.collider.gameObject.layer) & whatIsGround) != 0)
                    return new RaycastHit2D();

                if (hit.collider is not null && ((1 << hit.collider.gameObject.layer) & whatIsPlayer) != 0)
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
}
