using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI
{
    public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool unlocked;
        
        [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;
        
        [SerializeField] private Image skillImage;
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;
        [SerializeField] private Color lockedColor;
        [SerializeField] private Color unLockedColor;

        private UI ui;

        private void OnValidate()
        {
            gameObject.name = $"Skill - {skillName}";
        }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(UnlockSkill);
        }

        private void Start()
        {
            ui = GetComponentInParent<UI>();
            skillImage = GetComponent<Image>();
            skillImage.color = lockedColor;
        }

        private void Update()
        {
            if (CanUnlockSkill() && !unlocked)
            {
                skillImage.color = unLockedColor;
            }
        }

        public void UnlockSkill()
        {
            if (!CanUnlockSkill())
            {
                return;
            }

            unlocked = true;
            skillImage.color = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector2 mousePosition = Input.mousePosition;
                
            var xOffset = 50;
            var yOffset = 100;

            if (mousePosition.x > Screen.width * 0.5f)
            {
                xOffset *= -1;
            }

            if (mousePosition.y > Screen.height * 0.5f)
            {
                yOffset *= -1;
            }
            
            if (CanUnlockSkill())
            {
                ui.skillTooltip.ShowTooltip(skillDescription, skillName);
                ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
            }
            else
            {
                ui.skillTooltip.ShowTooltip("you need to unlock preview skill before see the description", "Unknown");
                ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.skillTooltip.HideTooltip();
        }

        private bool CanUnlockSkill()
        {
            if (shouldBeUnlocked.Any(item => !item.unlocked))
            {
                return false;
            }

            if (shouldBeLocked.Any(item => item.unlocked))
            {
                return false;
            }
            
            return true;
        }
    }
}