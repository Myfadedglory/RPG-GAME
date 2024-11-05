using System.Collections;
using UnityEngine;

namespace Script
{
    public class ChilledEffect : IElementalEffect
    {
        private readonly float duration;
        
        public ChilledEffect(float duration)
        {
            this.duration = duration;
        }

        public void ApplyEffect(CharacterStats target)
        {
            target.StartCoroutine(ApplyChillEffect(target));
        }

        private IEnumerator ApplyChillEffect(CharacterStats target)
        {
            var chillEffect = new Modifier("Chill Effect", Modifier.Operation.Multiplication, 0.5);
            target.physicsResistance.AddModifier(chillEffect);
            yield return new WaitForSeconds(duration);
            target.physicsResistance.RemoveModifier(chillEffect);
            target.CurrentStatus = ElementalStatus.None;
        }
    }
}