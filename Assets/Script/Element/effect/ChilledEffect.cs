using System.Collections;
using Script.Stats;
using UnityEngine;

namespace Script.Element.effect
{
    public class ChilledEffect : ElementalEffectBase
    {
        private readonly float actionSlowPercentage;
        private readonly Modifier chillEffect;

        public ChilledEffect(float duration, float actionSlowPercentage, float physicalResistanceReduceDegree) 
            : base(duration)
        {
            this.actionSlowPercentage = actionSlowPercentage;
            chillEffect = new Modifier("Element Effect", StatType.IceMagic, Modifier.Operation.Multiplication, physicalResistanceReduceDegree);
        }

        protected override IEnumerator EffectCoroutine()
        {
            Target.ApplyModifier(chillEffect);
            Target.GetComponentInParent<Enemy.Enemy>().SlowEntityFor(actionSlowPercentage, Duration);

            yield return new WaitForSeconds(Duration);

            Target.physicsResistance.RemoveModifier(chillEffect);
            Target.CurrentStatus = ElementalStatus.None;
        }
    }
}