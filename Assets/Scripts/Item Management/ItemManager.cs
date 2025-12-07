using UnityEngine;

public class ItemManager : MonoBehaviour
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
    public string itemName, itemDescription, itemEffectDescription;
    public int itemLevelRequirement, itemHPBoost, itemMPBoost, itemStrengthBoost,
    itemIntelligenceBoost, itemCritBoost, itemPhysicalDEFBoost, itemMagicDEFBoost;
    public Sprite itemIcon;
    public int itemID, itemValue, maxStackSize, currentStackSize;
    public bool isStackable;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Inventory.instance != null)
            {
                Debug.Log("Item was added -> " + Inventory.instance.itemWasAdded);
                Inventory.instance.itemWasAdded = false;
                gameObject.SetActive(false);
                Inventory.instance.AddItem(this);
                Debug.Log("Trigger ==> 1 <==");
                DestroyItem();
                Debug.Log("Trigger ==> 2 <==");
            }
            else
            {
                Debug.Log($"Inventory is = null!");
            }
        }
    }

    private void DestroyItem()
    {
        if (Inventory.instance.InventoryItemCount == MenuManager.instance.AvailableInventorySlots && !Inventory.instance.itemWasAdded)
            return;
        Destroy(gameObject);
    }


    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;

    public int CurrentStackSize
    {
        get => currentStackSize;
        set => currentStackSize = value;
    }
}
