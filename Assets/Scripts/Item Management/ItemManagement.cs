using UnityEngine;
using System;
namespace DefaultNamespace
{

    [CreateAssetMenu(fileName = "ItemManagement", menuName = "Scriptable Objects/ItemManagement")]
    public class ItemManagement : ScriptableObject, ISerializationCallbackReceiver
    {
        public enum ItemForQuest { QuestItem, NotForQuest }
        public enum ItemType { Consumable, Equipment, Miscellaneous, CraftingMaterial }
        public enum ItemAffinity { Magical, Physical, None }
        public enum ItemMaterial { Metal, Leather, Cloth, None }
        public enum ItemEffect { ExperienceAffector, HealthAffector, ManaAffector, StrengthAffector, None }
        public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary }
        public enum EquipmentType { Head, Chest, Arms, Legs, Weapon, Shield, None }
        public enum EquipSlotNeeded { OneHanded, TwoHanded, None }
        public ItemForQuest itemForQuest;
        public ItemType itemType;
        public ItemAffinity itemAffinity;
        public ItemMaterial itemMaterial;
        public ItemEffect itemEffect;
        public ItemRarity itemRarity;
        public EquipmentType equipmentType;
        public EquipSlotNeeded equipSlotNeeded;
        public Sprite itemIcon;
        public string itemName;
        public int itemID;
        public string itemDescription;
        public bool isStackable;
        public int itemValue, levelRequirement, maxStackSize, currentStackSize,
        hPBoost, mPBoost, strengthBoost, intelligenceBoost, itemCritBoost, itemPhysicalDEFBoost, magicDEFBoost;
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
