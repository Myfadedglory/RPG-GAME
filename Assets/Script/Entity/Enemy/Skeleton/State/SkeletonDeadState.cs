using System.Collections;
using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonDeadState : SkeletonState
    {
        public SkeletonDeadState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);
            Entity.StartCoroutine(DestroyAfterAnimation(Entity.deathTime));
        }

        public override void Update()
        {
            base.Update();

            Enemy.SetZeroVelocity();
        }
        
        private IEnumerator DestroyAfterAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            Object.Destroy(Entity.gameObject);
        }
    }
}
