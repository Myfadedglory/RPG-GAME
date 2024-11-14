using System.Collections;
using Script.Stats;

namespace Script.Element
{
    public abstract class ElementalEffectBase : IElementalEffect
    {
        protected readonly float Duration;
        protected CharacterStats Target;

        protected ElementalEffectBase(float duration)
        {
            Duration = duration;
        }

        public void ApplyEffect(CharacterStats target)
        {
            Target = target;
            target.StartCoroutine(EffectCoroutine());
        }

        protected abstract IEnumerator EffectCoroutine();
    }
}