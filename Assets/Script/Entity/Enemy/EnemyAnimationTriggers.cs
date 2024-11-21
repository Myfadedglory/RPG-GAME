using Script.Utilities;

namespace Script.Entity.Enemy
{
    public class EnemyAnimationTriggers : AnimationTriggers<Enemy>
    {
        protected void OpenCounterWindow() => Entity.OpenCounterAttackWindow();

        protected void CloseCounterWindow() => Entity.CloseCounterAttackWindow();
    }
}
