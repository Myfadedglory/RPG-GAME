using Script.Entity.Player;
using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonGroundedState : SkeletonState
    {
        private Transform player;

        protected SkeletonGroundedState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            player = PlayerManager.instance.player.transform;
        }

        public override void Update()
        {
            base.Update();

            if(Enemy.IsPlayerDetected() || Vector2.Distance(Enemy.transform.position , player.position) < Enemy.minDetectedDistance) 
                Fsm.SwitchState(Enemy.BattleState);
        }
    }
}
