using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerCatchSwordState : PlayerState
    {
        private Transform sword;

        public PlayerCatchSwordState(Script.Entity.Player.Player player, Fsm fsm, string animBoolName) : base(player, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            sword = Entity.Sword.transform;

            if (sword.position.x < Entity.transform.position.x && Entity.FacingDir == 1)
                Entity.Flip();
            else if (sword.position.x > Entity.transform.position.x && Entity.FacingDir == -1)
                Entity.Flip();

            Entity.SetVelocity(Entity.swordReturnForce * -Entity.FacingDir ,Rb.velocity.y  , false);
        }

        public override void Update()
        {
            base.Update();

            if(IsAnimationFinished)
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
