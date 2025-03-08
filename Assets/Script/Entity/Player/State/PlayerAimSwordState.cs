using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(Player player, Fsm fsm, string animBoolName) : base(player, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            Entity.Skill.Sword.ActiveDots(true);
        }

        public override void Update()
        {
            base.Update();

            Entity.SetXZeroVelocity();

            if (Input.GetKeyUp(KeyCode.Mouse1)) 
                Fsm.SwitchState(Entity.IdleState);

            Vector2 mousePosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosition.x < Entity.transform.position.x && Entity.FacingDir == 1)
                Entity.Flip();
            else if(mousePosition.x > Entity.transform.position.x && Entity.FacingDir != 1)
                Entity.Flip();
        }
    }
}
