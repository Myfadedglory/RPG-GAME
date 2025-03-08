using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerHitState : PlayerState
    {
        public PlayerHitState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = Entity.playerConfig.hitDuration;
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);
            BusyFor(0.2f);
        }

        public override void Update()
        {
            base.Update();

            if (IsAnimationFinished) 
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
