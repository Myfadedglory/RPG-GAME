using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerCounterAttackState : PlayerState
    {
        private static readonly int CounterSuccess = Animator.StringToHash("CounterSuccess");

        public PlayerCounterAttackState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = Entity.playerConfig.counterAttackDuration;

            Entity.Anim.SetBool(CounterSuccess, false);
        }

        public override void Update()
        {
            base.Update();

            Entity.SetZeroVelocity();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(Entity.attackCheck.position, Entity.attackCheckDistance);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy.Enemy>() != null)
                    if (hit.GetComponent<Enemy.Enemy>().CanBeStun())
                    {
                        StateTimer = 10;    //�����壬ֻ��һ���Ƚϴ��ֵ
                        Entity.Anim.SetBool(CounterSuccess, true);
                    }
            }

            if (StateTimer < 0 || IsAnimationFinished)
                Fsm.SwitchState(Entity.IdleState);

            if (!IsAnimationFinished && Input.GetKeyDown(KeyCode.Q))
                return;
        }
    }
}
