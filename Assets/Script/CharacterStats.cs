using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Major stats")]
        public Stat strength; // 力量
        public Stat agility;  // 敏捷
        public Stat intelligence; // 智力
        public Stat vitality; // 生命

        [Header("Offensive stats")]
        public Stat damage;
        public Stat critChance;
        public Stat critPower;

        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion; // 闪避
        public Stat physicsResistance;
        public Stat fireResistance;
        public Stat iceResistance;
        public Stat lightningResistance;

        [Header("Magic stats")]
        public Stat fireDamage;
        public Stat iceDamage;
        public Stat lightningDamage;

        [SerializeField] private float igniteDuration = 10f;
        [SerializeField] private float chillDuration = 20f;
        [SerializeField] private float shockDuration = 20f;

        [SerializeField] private double igniteDamage = 10;
        private const float IgniteDamageCoolDown = 1f;
        private float igniteDamageTimer;

        private enum ElementalStatus { None, Ignited, Chilled, Shocked }
        private ElementalStatus currentStatus;
        
        [SerializeField] private double currentHealth;
        
        private readonly Dictionary<ElementalStatus, float> statusTimers = new();

        protected virtual void Start()
        {
            currentStatus = ElementalStatus.None;
            currentHealth = maxHealth.GetValue();
        }

        protected virtual void Update()
        {
            UpdateStatusTimers();
            ApplyIgnite();
        }
        
        private float GetStatusDuration(ElementalStatus status)
        {
            return status switch
            {
                ElementalStatus.Ignited => igniteDuration,
                ElementalStatus.Chilled => chillDuration,
                ElementalStatus.Shocked => shockDuration,
                _ => 0f
            };
        }

        private void UpdateStatusTimers()
        {
            var keys = new List<ElementalStatus>(statusTimers.Keys);

            foreach (var status in keys)
            {
                statusTimers[status] -= Time.deltaTime;
                
                if (!(statusTimers[status] <= 0)) continue;
                
                statusTimers[status] = 0;
                
                if (currentStatus == status) currentStatus = ElementalStatus.None;
            }
        }

        #region ElementalStatus effect

        private void ApplyIgnite()
        {
            igniteDamageTimer -= Time.deltaTime;

            if (currentStatus != ElementalStatus.Ignited || !(igniteDamageTimer <= 0)) return;
            
            TakeDamage(igniteDamage);
            
            igniteDamageTimer = IgniteDamageCoolDown;
        }
        
        private bool ApplyShocked()
        {
            if (currentStatus != ElementalStatus.Shocked) return false;
            
            return Random.Range(0, 100) > vitality.GetValue();
        }

        #endregion

        public virtual void DoDamage(CharacterStats target, bool isMagic)
        {
            if (ApplyShocked()) return;
            
            if (!isMagic && ApplyEvasion(target)) return; // 仅在物理攻击时检查闪避

            double totalDamage;

            if (isMagic)
            {
                var fire = ApplyResistance(fireDamage.GetValue(), target.fireResistance);
                var ice = ApplyResistance(iceDamage.GetValue(), target.iceResistance);
                var lightning = ApplyResistance(lightningDamage.GetValue(), target.lightningResistance);

                target.SetMainMagicStatus(fire, ice, lightning);
        
                totalDamage = fire + ice + lightning;
                
                totalDamage += intelligence.GetValue() * totalDamage / 100;
            }
            else
            {
                totalDamage = CalculateTotalDamage(damage.GetValue(), strength.GetValue());
                
                totalDamage = ApplyCrit(totalDamage);

                if (target.currentStatus != ElementalStatus.Chilled)
                {
                    totalDamage = ApplyResistance(totalDamage, target.physicsResistance);
                }
            }

            target.TakeDamage(totalDamage);
        }

        private void SetMainMagicStatus(double fire, double ice, double lightning)
        {
            currentStatus = DetermineMainMagicStatus(fire, ice, lightning);
            
            statusTimers[currentStatus] = GetStatusDuration(currentStatus);
        }

        private static ElementalStatus DetermineMainMagicStatus(double fire, double ice, double lightning)
        {
            if (fire <= 0 && ice <= 0 && lightning <= 0) return ElementalStatus.None;

            if (fire > ice && fire > lightning) return ElementalStatus.Ignited;
            if (ice > fire && ice > lightning) return ElementalStatus.Chilled;
            if (lightning > fire && lightning > ice) return ElementalStatus.Shocked;

            while (true)
            {
                if (Random.value < 0.33 && fire > 0) return ElementalStatus.Ignited;
                if (Random.value < 0.5 && ice > 0) return ElementalStatus.Chilled;
                if (lightning > 0) return ElementalStatus.Shocked;
            }
        }

        private static double CalculateTotalDamage(double baseDamage, double mainStatValue)
        {
            return baseDamage + mainStatValue;
        }

        #region Attribute Correction

        private static double ApplyResistance(double baseDamage, Stat resistance)
        {
            return baseDamage * (1 - resistance.GetValue());
        }
        
        private double ApplyCrit(double baseDamage)
        {
            if (Random.Range(1, 100) < (critChance.GetValue() + agility.GetValue()) * 100)
            {
                baseDamage *= 1 + critPower.GetValue();
            }
            return baseDamage;
        }

        private static bool ApplyEvasion(CharacterStats target)
        {
            return Random.Range(1, 100) < target.evasion.GetValue() + target.agility.GetValue();
        }

        #endregion

        protected virtual void TakeDamage(double damageAmount)
        {
            currentHealth -= damageAmount;
            
            if (currentHealth <= 0) Die();
        }

        protected virtual void Die()
        {
        }
    }
}
