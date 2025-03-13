using System;
using Script.Skill;
using UnityEngine;

namespace Script.Config
{
    [Serializable]
    public class CloneConfig
    {
        [Header("Clone info")]
        public GameObject prefab;
        public float duration;
        public float detectDistance;
        
        [Header("Duplicate clone info")]
        public float chanceToDuplicate;
        
        [Header("Clone SkillCondition")]
        public SkillCondition clone;
        public SkillCondition cloneAttack;
        public SkillCondition cloneCrystal;
        public SkillCondition duplicateClone;
    }
}