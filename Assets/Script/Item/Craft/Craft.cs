using System.Collections.Generic;
using System.Linq;
using Script.Item.Equipment;
using Script.UI;
using TMPro;
using UnityEngine;

namespace Script.Item.Craft
{
    public class Craft : MonoBehaviour
    {
        [Header("Equipment Data Lists")]
        [SerializeField] private List<EquipmentData> weapons = new List<EquipmentData>();
        [SerializeField] private List<EquipmentData> armors = new List<EquipmentData>();
        [SerializeField] private List<EquipmentData> amulets = new List<EquipmentData>();
        [SerializeField] private List<EquipmentData> flasks = new List<EquipmentData>();

        [Header("UI References")]
        [SerializeField] private GameObject itemPrefab; // 物品预制体
        [SerializeField] private Transform craftList;   // 生成物品的父节点
        [SerializeField] private Transform typeSwitch;

        private UI.UI ui;

        /// <summary>
        /// 清除所有已生成的物品
        /// </summary>
        private void ClearItemPrefab()
        {
            if (craftList == null) return;
            
            if (craftList.childCount == 0) return;

            // 反向遍历避免修改集合时的顺序问题
            for (var i = craftList.childCount - 1; i >= 0; i--)
            {
                Destroy(craftList.GetChild(i).gameObject);
            }
            
            if(ui.craftTooltip == null) return;
            
            ui.craftTooltip.HideTooltip();
        }

        private void Start()
        {
            GenerateWeaponCraft();
            ui = GetComponentInParent<UI.UI>();
        }

        /// <summary>
        /// 初始化单个物品的UI
        /// </summary>
        private void InitializeItemPrefab(EquipmentData equipmentData)
        {
            if (itemPrefab == null || craftList == null) return;

            var craftSlot = Instantiate(itemPrefab, craftList.transform);
            var slotUI = craftSlot.GetComponent<CraftSlotUI>();

            if (slotUI != null)
            {
                slotUI.CopyFrom(equipmentData);
            }
            else
            {
                Debug.LogWarning("CraftSlotUI component missing on itemPrefab!");
            }
        }

        //----- 各类型装备生成方法 -----//
        public void GenerateWeaponCraft()
        {
            ClearItemPrefab();
            GenerateCraftItems(GetUnlockedEquipment(weapons));
        }

        public void GenerateArmorCraft()
        {
            ClearItemPrefab();
            GenerateCraftItems(GetUnlockedEquipment(armors));
        }

        public void GenerateAmuletCraft()
        {
            ClearItemPrefab();
            GenerateCraftItems(GetUnlockedEquipment(amulets));
        }

        public void GenerateFlaskCraft()
        {
            ClearItemPrefab();
            GenerateCraftItems(GetUnlockedEquipment(flasks));
        }

        //----- 核心逻辑封装 -----//
        private void GenerateCraftItems(List<EquipmentData> equipmentList)
        {
            if (equipmentList.Count == 0) return;

            foreach (var equipment in equipmentList)
            {
                InitializeItemPrefab(equipment);
            }
        }

        private static List<EquipmentData> GetUnlockedEquipment(List<EquipmentData> equipments)
        {
            return equipments.Where(equipment => !equipment.locked).ToList();
        }

        private static void SetTextColor(Color color, GameObject button)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>() == null) return;
            
            button.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }

        public void ChooseFor(GameObject button)
        {
            if(typeSwitch.childCount == 0) return;
            
            for (var i = typeSwitch.childCount - 1; i >= 0; i--)
            {
                SetTextColor(Color.white, typeSwitch.GetChild(i).gameObject);
            }
            
            SetTextColor(Color.yellow, button);
        }
    }
}