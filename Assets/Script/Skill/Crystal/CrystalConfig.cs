using System;
using UnityEngine;

namespace Script.Skill.Crystal
{
    [Serializable]
    public class CrystalConfig
    {
        public GameObject prefab;
        public float duration;
        public Vector2 maxSize;
        public float growSpeed;
        
        [Header("Crystal Movement")]
        public float moveSpeed;
        public float detectDistance;
        
        [Header("Multiple Crystal")]
        public int crystalStackAmount;
        public int multipleCooldown;
        public float explodeDistance;
        public float useMultipleWindow;
        
        [Header("Crystal SkillCondition")]
        public SkillCondition crystal;
        public SkillCondition crystalMove;
        public SkillCondition crystalExplode;
        public SkillCondition multipleCrystal;
        public SkillCondition crystalMirage;
    }
}