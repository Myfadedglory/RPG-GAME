using Script.Utilities;

namespace Script.Enemy
{
    public class EnemyAnimationTriggers : AnimationTriggers<Enemy>
    {
        protected void OpenCounterWindow() => Entity.OpenCounterAttackWindow();

        protected void CloseCounterWindow() => Entity.CloseCounterAttackWindow();
    }
}
