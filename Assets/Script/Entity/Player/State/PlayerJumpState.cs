using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerJumpState : PlayerAirState
    {
        public PlayerJumpState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            Rb.velocity = new Vector2(Rb.velocity.x, Entity.jumpForce.y);
        }

        public override void Update()
        {
            base.Update();

            if (Rb.velocity.y > 0)
                Fsm.SwitchState(Entity.AirState);
        }
    }
}
