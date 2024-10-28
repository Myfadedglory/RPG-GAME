using Script.Utilities;

namespace Script.Enemy.Skeleton.State
{
    public class SkeletonState : EnemyState
    {
        protected readonly Skeleton Enemy;

        protected SkeletonState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName)
        {
            Enemy = enemy;
        }
    }
}
