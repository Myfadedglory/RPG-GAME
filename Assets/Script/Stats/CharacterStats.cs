using System;
using System.Collections.Generic;
using Script.Element;
using Script.Element.effect;
using Script.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        #region Stats
        
        /// <summary>
        /// strength         physicsDamage                   type : double   range : (0, infinity)
        /// agility          evasion                         type : double   range : (0, infinity)
        /// intelligence     magicDamage                     type : double   range : (0, infinity)
        /// vitality         maxHealth,shocked resistance    type : double   range : (0, infinity) 
        /// </summary>
        
        [Header("Major stats")]
        public ValueStat strength;     
        public ValueStat agility;      
        public ValueStat intelligence; 
        public ValueStat vitality;     
        
        /// <summary>
        /// physicsDamage      basic damage                  type : double   range : (0, infinity)
        /// attackAccurate     accurate of do damage         type : double   range : (0, 1)
        /// critChance         the chance of crit            type : double   range : (0, 1)
        /// critPower          critDamage                    type : double   range : (0, infinity)
        /// </summary>
        
        [Header("Offensive stats")]
        public Stat physicsDamage;
        public Stat attackAccurate;
        public Stat critChance;    
        public ValueStat critPower;     
        
        /// <summary>
        /// maxHealth          max health                    type : double   range : (0, infinity)
        /// evasion            chance of evasion             type : double   range : (0, 100)
        /// armor              decrease damage               type : double   range : (0, infinity)
        /// physicsResistance  free % of physics damage      type : double   range : (0, 1)
        /// </summary>

        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat evasion;
        public ValueStat armor;
        public ValueStat physicsResistance;
        
        /// <summary>
        /// fireMagic          fire magic damage             type : double   range : (0, infinity)
        /// iceMagic           ice magic damage              type : double   range : (0, infinity)
        /// lightningMagic     lightning magic damage        type : double   range : (0, infinity)
        /// </summary>
        
        [Header("Magic stats")] 
        public MagicStat fireMagic;
        public MagicStat iceMagic;
        public MagicStat lightningMagic;
        
        #endregion
        
        #region  Element effect data
        
        private const double IgniteDamage = 5f;
        private const float IgniteDamageCoolDown = 1f;
        private const float ChillReducePhysicResistance = 0.5f;
        private const float ChillActionSlowPercentage = 0.5f;
        private const float ShockedReduceAttackAccurate = 0.5f;
        [SerializeField] private GameObject shockStrikePrefab;
        private const float ShockStrikeSpeed = 5;
        private const double ShockStrikeDamage = 5;
        private const float DetectEnemyRadius = 20;
        
        #endregion
        
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
            InitializeStatDictionary();
            OnHealthChanged?.Invoke();
        }
        
        #region Modifiers

        private Dictionary<StatType, Stat> statDictionary;

        private void InitializeStatDictionary()
        {
            statDictionary = new Dictionary<StatType, Stat>
            {
                { StatType.Strength, strength },
                { StatType.Agility, agility },
                { StatType.Intelligence, intelligence },
                { StatType.Vitality, vitality },
                { StatType.PhysicsDamage, physicsDamage },
                { StatType.AttackAccurate, attackAccurate },
                { StatType.CritChance, critChance },
                { StatType.CritPower, critPower },
                { StatType.MaxHealth, maxHealth },
                { StatType.Evasion, evasion },
                { StatType.Armor, armor },
                { StatType.FireMagic, fireMagic },
                { StatType.FireResistance, fireMagic.magicResistance },
                { StatType.IceMagic, iceMagic },
                { StatType.IceResistance, iceMagic.magicResistance },
                { StatType.LightningMagic, lightningMagic },
                { StatType.LightningResistance, lightningMagic.magicResistance }
            };
        }

        public virtual void ApplyModifier(Modifier modifier)
        {
            if (statDictionary.TryGetValue(modifier.GetStatType(), out var stat))
            {
                stat.AddModifier(modifier);
            }
        }

        public virtual void RemoveModifier(Modifier modifier)
        {
            if (statDictionary.TryGetValue(modifier.GetStatType(), out var stat))
            {
                stat.RemoveModifier(modifier);
            }
        }

        #endregion

        #region ElementalStatus Effect
        
        private void StatusEffect(ElementalStatus status, float statusDuration)
        {
            var colors = status switch
            {
                ElementalStatus.Ignited => fx.igniteColor,
                ElementalStatus.Chilled => fx.chillColor,
                ElementalStatus.Shocked => fx.lightningColor,
                _ => new List<Color> { Color.white, Color.white } // 默认颜色列表
            };

            IElementalEffect effect = status switch
            {
                ElementalStatus.Ignited => new IgniteEffect(statusDuration, IgniteDamage, IgniteDamageCoolDown),
                ElementalStatus.Chilled => new ChilledEffect(statusDuration, ChillActionSlowPercentage, ChillReducePhysicResistance),
                ElementalStatus.Shocked => new ShockedEffect(statusDuration, ShockedReduceAttackAccurate, transform, DetectEnemyRadius),
                _ => null
            };

            if (effect == null) return;
            
            effect.ApplyEffect(this);
            fx.AlimentsFxFor(colors, statusDuration);
        }


        
        private double SetMagic(CharacterStats target, MagicType magicType)
        {
            double totalDamage;
            
            switch (magicType)
            {
                case MagicType.Fire:
                    target.statusTimers = fireMagic.GetMagicStatusDuration();
                    totalDamage = target.fireMagic.magicResistance.ApplyStat(fireMagic.GetValue());
                    target.CurrentStatus = ElementalStatus.Ignited;
                    break;
                case MagicType.Ice:
                    target.statusTimers = iceMagic.GetMagicStatusDuration();
                    totalDamage = target.iceMagic.magicResistance.ApplyStat(iceMagic.GetValue());
                    target.CurrentStatus = ElementalStatus.Chilled;
                    break;
                case MagicType.Lightning:
                    target.statusTimers = lightningMagic.GetMagicStatusDuration();
                    totalDamage = target.lightningMagic.magicResistance.ApplyStat(lightningMagic.GetValue());
                    target.CurrentStatus = ElementalStatus.Shocked;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null);
            }

            return totalDamage;
        }
        
        public virtual void CreateShockStrike(CharacterStats target)
        {
            var shockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            
            shockStrike.GetComponent<ThunderStrikeController>().SetUp(target, ShockStrikeSpeed, ShockStrikeDamage);
        }

        #endregion

        #region DoDamage
        
        public virtual void DoMagicDamage(CharacterStats target, MagicType magicType)
        {
            if(Random.Range(0, 100) <= target.agility.ApplyStat(target.evasion.GetValue())||
               Random.Range(0, 100) <= attackAccurate.GetValue()) return;

            var damage = SetMagic(target, magicType);
            
            target.StatusEffect(target.CurrentStatus, target.statusTimers);
            
            damage = intelligence.ApplyStat(damage, 3);
            
            target.TakeDamage(damage);
        }
        
        public virtual void DoPhysicsDamage(CharacterStats target)
        {
            if (Random.Range(0, 100) <= target.agility.ApplyStat(target.evasion.GetValue(), 2)||
                Random.Range(0, 100) <= attackAccurate.GetValue()) return;

            var damage = strength.ApplyStat(physicsDamage.GetValue(), 2);

            if (Random.Range(0, 100) < critChance.GetValue() + strength.GetValue())
            {
                damage = critPower.ApplyStat(damage);
            }
                
            damage = target.armor.ApplyStat(damage);
            
            damage = target.physicsResistance.ApplyStat(damage);
            
            target.TakeDamage(damage);
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
            return vitality.ApplyStat(maxHealth.GetValue(), 5);
        }

        public double GetCurrentHealth()
        {
            return currentHealth;
        }

        #endregion

        protected virtual void Die()
        {
        }


        public Stat GetStat(StatType statType)
        {
            return statType switch
            {
                StatType.Strength => strength,
                StatType.Agility => agility,
                StatType.Intelligence => intelligence,
                StatType.Vitality => vitality,
                StatType.PhysicsDamage => physicsDamage,
                StatType.AttackAccurate => attackAccurate,
                StatType.CritChance => critChance,
                StatType.CritPower => critPower,
                StatType.MaxHealth => maxHealth,
                StatType.Evasion => evasion,
                StatType.Armor => armor,
                StatType.FireMagic => fireMagic,
                StatType.FireResistance => fireMagic.magicResistance,
                StatType.IceMagic => iceMagic,
                StatType.IceResistance => iceMagic.magicResistance,
                StatType.LightningMagic => lightningMagic,
                StatType.LightningResistance => lightningMagic.magicResistance,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
    }
}
