using Script.Skill.Sword;
using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(Script.Entity.Player.Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            if(isBusy || PlayerManager.instance.player.totalMenu.GetComponent<UI.UI>().UIOpenStatus()) return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                Fsm.SwitchState(Entity.BlackHole);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            {
                Fsm.SwitchState(Entity.AimSword);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Fsm.SwitchState(Entity.CounterAttack);
                return;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fsm.SwitchState(Entity.AttackState);
                return;
            }

            if (!Entity.IsGroundDetected())
            {
                Fsm.SwitchState(Entity.AirState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && Entity.IsGroundDetected())
            {
                Fsm.SwitchState(Entity.JumpState);
                return;
            }
        }

        private bool HasNoSword()
        {
            if(!Entity.Sword)
                return true;

            Entity.Sword.GetComponent<SwordSkillController>().ReturnSword();

            return false;
        }
    }
}
