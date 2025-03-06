using Script.Utilities;

namespace Script.Entity.Enemy
{
    public class EnemyState : EntityState<Enemy>
    {
        protected EnemyState(Enemy entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
    }
}
