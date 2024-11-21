using Script.Entity.Player;
using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonBattleState : SkeletonState
    {
        private Transform player;
        private int moveDir;

        public SkeletonBattleState(Enemy entity, Fsm fsm, string animBoolName,Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            Enemy.CloseCounterImage();

            player = PlayerManager.instance.player.transform;
        }
        
        public override void Update()
        {
            base.Update();

            if (Enemy.IsPlayerDetected())
            {
                StateTimer = Enemy.battleTime;

                if (Enemy.IsPlayerDetected().distance <= Enemy.attackDistance)
                {
                    Enemy.SetXZeroVelocity();

                    if (CanAttack())
                        Fsm.SwitchState(Enemy.AttackState);

                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(player.transform.position, Enemy.transform.position) > Enemy.hatredDistance)
                    Fsm.SwitchState(Enemy.IdleState);
            }

            if(Fsm.CurrentState != Enemy.AttackState)
            {
                if(Mathf.Approximately(player.position.x, Enemy.transform.position.x))
                    return;
                if (player.position.x > Enemy.transform.position.x)
                    moveDir = 1;               
                else if (player.position.x < Enemy.transform.position.x)
                    moveDir = -1;

                Enemy.SetVelocity(Enemy.moveSpeed * moveDir * Enemy.speedMultiplier, Rb.velocity.y, Enemy.needFlip);
            }
        }

        private bool CanAttack()
        {
            if (!(Time.time > Enemy.lastTimeAttacked + Enemy.attackCoolDown)) return false;
            
            Enemy.lastTimeAttacked = Time.time;

            return true;

        }
    }
}
