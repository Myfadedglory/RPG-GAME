using Script.Utilities;

namespace Script.Enemy
{
    public class EnemyAnimationTriggers : AnimationTriggers<Enemy>
    {
        protected void OpenCounterWindow() => entity.OpenCounterAttackWindow();

        protected void CloseCounterWindow() => entity.CloseCounterAttackWindow();
    }
}
