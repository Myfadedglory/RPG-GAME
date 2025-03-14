﻿using System;
using Script.Skill;
using UnityEngine;

namespace Script.Config
{
    [Serializable]
    public class BlackholeConfig
    {
        public int amountOfAttack = 4;
        public float cloneAttackCoolDown = 0.3f;
        [Space]
        public GameObject prefab;
        public Vector2 maxSize;
        public float duration = 20;
        public float growSpeed = 1f;
        public float shrinkSpeed = 3f;
        
        [Header("Skill Condition")] 
        public SkillCondition blackhole;
    }
}