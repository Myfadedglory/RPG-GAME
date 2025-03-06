using System;
using Script.UI;
using UnityEngine;

namespace Script.Skill
{
    [Serializable]
    public class SkillCondition
    {
        [SerializeField] private SkillTreeSlotUI skillTreeSlotUI;
        private bool skillCondition;
        
        public void UpdateCondition()
        {
            if (skillTreeSlotUI)
            {
                skillCondition = skillTreeSlotUI.unlocked; // 直接同步状态
            }
        }

        public bool GetSkillCondition()
        {
            UpdateCondition(); // 每次获取时检查状态
            return skillCondition;
        }
    }
}