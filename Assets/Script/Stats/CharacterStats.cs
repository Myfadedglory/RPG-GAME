using System;
using Script.Element;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script.Stats
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

        private const double IgniteDamage = 5f;
        private const float IgniteDamageCoolDown = 1f;
        private const float ChillReducePhysicResistance = 0.5f;
        private const float ChillActionSlowPercentage = 0.5f;
        private const float ShockedReduceAttackAccurate = 0.5f;
        [SerializeField] private GameObject shockStrikePrefab;
        private const float ShockStrikeSpeed = 5;
        private const double ShockStrikeDamage = 5;
        private const float DetectEnemyRadius = 20;
        
        public double currentHealth;
        public Action OnHealthChanged;

        internal ElementalStatus CurrentStatus;
        private float statusTimers;
        
        private EntityFX fx;

        protected virtual void Start()
        {
            CurrentStatus = ElementalStatus.None;
            currentHealth = GetMaxHealth();
            fx = GetComponentInParent<EntityFX>();
            OnHealthChanged?.Invoke();
        }

        #region ElementalStatus Effect
        
        private void StatusEffect(ElementalStatus status, float statusDuration)
        {
            switch (status)
            {
                case ElementalStatus.Ignited:
                    var igniteEffect = new IgniteEffect(statusDuration, IgniteDamage, IgniteDamageCoolDown);
                    igniteEffect.ApplyEffect(this);
                    fx.AlimentsFxFor(fx.igniteColor, statusDuration);
                    return;
                case ElementalStatus.Chilled:
                    var chillEffect = new ChilledEffect(statusDuration, ChillActionSlowPercentage, ChillReducePhysicResistance);
                    chillEffect.ApplyEffect(this);
                    fx.AlimentsFxFor(fx.chillColor, statusDuration);
                    return;
                case ElementalStatus.Shocked:
                    var shockedEffect = new ShockedEffect(statusDuration, ShockedReduceAttackAccurate, transform, DetectEnemyRadius);
                    shockedEffect.ApplyEffect(this);
                    fx.AlimentsFxFor(fx.lightningColor, statusDuration);
                    return;
                case ElementalStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private double SetMagic(CharacterStats target, MagicType magicType)
        {
            double totalDamage;
            
            switch (magicType)
            {
                case MagicType.Fire:
                    target.statusTimers = fireMagic.GetMagicStatusDuration();
                    totalDamage = ApplyResistance(fireMagic.GetValue(), target.fireMagic.magicResistance);
                    target.CurrentStatus = ElementalStatus.Ignited;
                    break;
                case MagicType.Ice:
                    target.statusTimers = iceMagic.GetMagicStatusDuration();
                    totalDamage = ApplyResistance(iceMagic.GetValue(), target.iceMagic.magicResistance);
                    target.CurrentStatus = ElementalStatus.Chilled;
                    break;
                case MagicType.Lightning:
                    target.statusTimers = lightningMagic.GetMagicStatusDuration();
                    totalDamage = ApplyResistance(lightningMagic.GetValue(), target.lightningMagic.magicResistance);
                    target.CurrentStatus = ElementalStatus.Shocked;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null);
            }

            return totalDamage;
        }

        #endregion

        public virtual void CreateShockStrike(CharacterStats target)
        {
            var shockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            
            shockStrike.GetComponent<ThunderStrikeController>().SetUp(target, ShockStrikeSpeed, ShockStrikeDamage);
        }

        #region DoDamage
        
        public virtual void DoMagicDamage(CharacterStats target, MagicType magicType)
        {
            if(ApplyEvasion(target) || ApplyAttackAccurate()) return;

            var totalDamage = SetMagic(target, magicType);
            
            target.StatusEffect(target.CurrentStatus, target.statusTimers);
            
            totalDamage *= 1 + intelligence.GetValue() * totalDamage / 100;
            
            target.TakeDamage(totalDamage);
        }
        
        public virtual void DoPhysicsDamage(CharacterStats target)
        {
            if (ApplyEvasion(target) || ApplyAttackAccurate()) return;

            var totalDamage = physicsDamage.GetValue() + strength.GetValue();
                
            totalDamage = ApplyCrit(totalDamage);
                
            totalDamage = ApplyResistance(totalDamage, target.physicsResistance);
            
            target.TakeDamage(totalDamage);
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
            fx.StartCoroutine("FlashFX");
            
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
