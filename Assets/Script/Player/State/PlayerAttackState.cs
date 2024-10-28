using Script.Utilities;
using UnityEngine;

namespace Script.Player.State
{
    public class PlayerAttackState : PlayerState
    {
        private static readonly int ComboCounter = Animator.StringToHash("ComboCounter");
        private int comboCounter;

        private float lastTimeAttacked;

        public PlayerAttackState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = .1f;

            if (comboCounter > 2 || Time.time >= lastTimeAttacked + Entity.comboWindow)
                comboCounter = 0;

            Entity.SetVelocity(
                Entity.attackMoveMent[comboCounter].x * Entity.FacingDir, 
                Entity.attackMoveMent[comboCounter].y , Entity.needFlip
            );

            Anim.SetInteger(ComboCounter, comboCounter);

            Anim.speed = Entity.attackSpeed;
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);

            BusyFor(0.15f);

            Anim.speed = 1;

            lastTimeAttacked = Time.time;

            comboCounter++;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                Entity.SetZeroVelocity();

            if (IsAnimationFinished)
                Fsm.SwitchState(Entity.IdleState);        
        }
    }
}
