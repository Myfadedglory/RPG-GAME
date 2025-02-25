using Script.Item.Equipment;
using Script.UI;
using TMPro;
using UnityEngine;

namespace Script.Item.Craft
{
    public class CraftTooltip : Tooltip
    {
        [SerializeField] private TextMeshProUGUI unLockedCondition;

        /// <summary>
        /// 制作物品的详细信息
        /// </summary>
        public override void ShowTooltip(EquipmentData item)
        {
            base.ShowTooltip(item);
            unLockedCondition.text = item.GetRawMaterialDescription();
        }
        
        public void Craft()
        {
            var craftData = CurrentEquipment;
            
            if (craftData != null && Inventory.Inventory.instance.CanCraft(craftData, craftData.craftMaterials))
            {
                Debug.Log("you create a craft");
            }
        }
    }
}