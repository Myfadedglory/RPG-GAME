using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class CharacterStats : MonoBehaviour
    {
        #region Stats
        
        /*
         * strength         physicsDamage                   type : double   range : (0, infinity)
         * agility          evasion                         type : double   range : (0, infinity)
         * intelligence     magicDamage                     type : double   range : (0, infinity)
         * vitality         maxHealth,shocked resistance    type : double   range : (0, infinity)
         */
        [Header("Major stats")]
        public Stat strength;     
        public Stat agility;      
        public Stat intelligence; 
        public Stat vitality;     
        
        /*
         * physicsDamage      basic damage                  type : double   range : (0, infinity)
         * attackAccurate     accurate of do damage         type : double   range : (0, 1)
         * critChance         the chance of crit            type : double   range : (0, 1)
         * critPower          critDamage                    type : double   range : (0, infinity)
         */
        
        [Header("Offensive stats")]
        public Stat physicsDamage;
        public Stat attackAccurate;
        public Stat critChance;    
        public Stat critPower;     
        
        /*
         * maxHealth          max health                    type : double   range : (0, infinity)
         * evasion            chance of evasion             type : double   range : (0, 100)
         * physicsResistance  free % of physics damage      type : double   range : (0, 1)
         */

        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat evasion; 
        public Stat physicsResistance;

        /*
         * fireMagic          fire magic damage             type : double   range : (0, infinity)
         * iceMagic           ice magic damage              type : double   range : (0, infinity)
         * lightningMagic     lightning magic damage        type : double   range : (0, infinity)
         */
        
        [Header("Magic stats")] 
        public MagicStat fireMagic;
        public MagicStat iceMagic;
        public MagicStat lightningMagic;
        
        #endregion
        
        private const float IgniteDamageCoolDown = 1f;
        private float igniteDamageTimer;
        
        public double currentHealth;
        public Action OnHealthChanged;

        internal ElementalStatus CurrentStatus;
        private readonly Dictionary<ElementalStatus, float> statusTimers = new();
        
        private EntityFX fx;

        protected virtual void Start()
        {
            CurrentStatus = ElementalStatus.None;
            currentHealth = GetMaxHealth();
            fx = GetComponentInParent<EntityFX>();
            OnHealthChanged?.Invoke();
        }

        protected virtual void Update()
        {
            UpdateStatusTimers();
            ApplyIgnite();
        }

        #region StatusTimer
        
        private float GetStatusTimer(ElementalStatus status)
        {
            return status switch
            {
                ElementalStatus.Ignited => fireMagic.GetMagicStatusDuration(),
                ElementalStatus.Chilled => iceMagic.GetMagicStatusDuration(),
                ElementalStatus.Shocked => lightningMagic.GetMagicStatusDuration(),
                _ => 0f
            };
        }

        private void UpdateStatusTimers()
        {
            var keys = new List<ElementalStatus>(statusTimers.Keys);

            foreach (var status in keys)
            {
                if (statusTimers[status] <= 0)
                {
                    statusTimers[status] = 0;

                    if (CurrentStatus == status)
                    {
                        CurrentStatus = ElementalStatus.None;
                    }
                    continue;
                }
                
                statusTimers[status] -= Time.deltaTime;
            }
        }

        #endregion

        #region ElementalStatus effect

        private void ApplyIgnite()
        {
            igniteDamageTimer -= Time.deltaTime;

            if (CurrentStatus != ElementalStatus.Ignited || igniteDamageTimer > 0) return;
            
            TakeDamage(maxHealth.GetValue() / 100);
            
            igniteDamageTimer = IgniteDamageCoolDown;
        }
        
        private bool ApplyShocked()
        {
            if (CurrentStatus != ElementalStatus.Shocked) return false;
            
            return Random.Range(0, 100) > vitality.GetValue();
        }

        private IEnumerator ApplyChill(float duration)
        {
            var chillEffect = new Modifier("Chill Effect", Modifier.Operation.Multiplication, 0.5);
            physicsResistance.AddModifier(chillEffect);
            yield return new WaitForSeconds(duration);
            physicsResistance.RemoveModifier(chillEffect);
        }

        private void StatusColorEffect(ElementalStatus status, float statusDuration)
        {
            switch (status)
            {
                case ElementalStatus.Ignited:
                    fx.AlimentsFxFor(fx.igniteColor, statusDuration);
                    return;
                case ElementalStatus.Chilled:
                    fx.AlimentsFxFor(fx.chillColor, statusDuration);
                    return;
                case ElementalStatus.Shocked:
                    fx.AlimentsFxFor(fx.lightningColor, statusDuration);
                    return;
                case ElementalStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public virtual void DoDamage(CharacterStats target, bool isMagic)
        {
            if (!isMagic && ApplyEvasion(target) && ApplyShocked() && ApplyAttackAccurate()) return; 

            double totalDamage;

            if (isMagic)
            {
                var fire = ApplyResistance(fireMagic.GetValue(), target.fireMagic.magicResistance);
                var ice = ApplyResistance(iceMagic.GetValue(), target.iceMagic.magicResistance);
                var lightning = ApplyResistance(lightningMagic.GetValue(), target.lightningMagic.magicResistance);

                target.SetMainMagicStatus(fire, ice, lightning);
                
                target.statusTimers[target.CurrentStatus] = GetStatusTimer(target.CurrentStatus);
                
                target.StatusColorEffect(target.CurrentStatus, target.statusTimers[target.CurrentStatus]);
        
                totalDamage = fire + ice + lightning;
                
                totalDamage *= 1 + intelligence.GetValue() * totalDamage / 100;
            }
            else
            {
                totalDamage = physicsDamage.GetValue() + strength.GetValue();
                
                totalDamage = ApplyCrit(totalDamage);

                /*
                 * 对于寒冷效果，这里简单采用了免除物理抗性的效果
                 */
                
                totalDamage = ApplyResistance(totalDamage, target.physicsResistance);
            }
            
            target.TakeDamage(totalDamage);
        }

        #region GetMainMagicStatus

        private void SetMainMagicStatus(double fire, double ice, double lightning)
        {
            CurrentStatus = DetermineMainMagicStatus(fire, ice, lightning);
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

        #endregion

        #region Attribute Correction

        private bool ApplyAttackAccurate()
        {
            return Random.value > attackAccurate.GetValue();
        }

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

        public virtual void TakeDamage(double damage)
        {
            currentHealth -= damage;

            OnHealthChanged?.Invoke();
            
            if (currentHealth <= 0) Die();
        }
        
        #region GetValue

        public double GetMaxHealth()
        {
            return maxHealth.GetValue() + vitality.GetValue() * 5;
        }

        public double GetCurrentHealth()
        {
            return currentHealth;
        }

        #endregion

        protected virtual void Die()
        {
        }
    }
}
