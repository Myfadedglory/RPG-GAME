using Script.Utilities;

namespace Script.Entity.Enemy
{
    public class EnemyState : EntityState<Enemy>
    {
        protected EnemyState(Script.Entity.Enemy.Enemy entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
    }
}
