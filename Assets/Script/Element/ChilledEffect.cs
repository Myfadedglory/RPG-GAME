using System.Collections;
using Script.Stats;
using UnityEngine;

namespace Script.Element
{
    public class ChilledEffect : IElementalEffect
    {
        private readonly float duration;
        private readonly float actionSlowPercentage;
        private readonly Modifier chillEffect;
        
        public ChilledEffect(float duration, float actionSlowPercentage, float physicalResistanceReduceDegree)
        {
            this.duration = duration;
            this.actionSlowPercentage = actionSlowPercentage;
            chillEffect = new Modifier("Element Effect", Modifier.Operation.Multiplication, physicalResistanceReduceDegree);
        }

        public void ApplyEffect(CharacterStats target)
        {
            target.StartCoroutine(ApplyChillEffect(target));
            target.GetComponentInParent<Enemy.Enemy>().SlowEntityFor(actionSlowPercentage, duration);
        }

        private IEnumerator ApplyChillEffect(CharacterStats target)
        {
            target.physicsResistance.AddModifier(chillEffect);
            
            yield return new WaitForSeconds(duration);
            
            target.physicsResistance.RemoveModifier(chillEffect);
            
            target.CurrentStatus = ElementalStatus.None;
        }
    }
}