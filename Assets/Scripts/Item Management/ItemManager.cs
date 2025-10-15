using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public enum ItemForQuest { QuestItem, NotForQuest }
    public ItemForQuest itemForQuest;
    public enum ItemType
    {
        Consumable,
        Equipment,
        Miscellaneous,
        CraftingMaterial

    }

    public ItemType itemType;
    public enum ItemAffinity { Magical, Physical, None }
    public ItemAffinity itemAffinity;

    public enum ItemMaterial { Metal, Leather, Cloth, None }
    public ItemMaterial itemMaterial;
    public string itemName, itemDescription;
    public Sprite itemIcon;
    public int itemID, itemValue, maxStackSize, currentStackSize;
    public bool isStackable;

    public enum ItemEffect
    {
        ExperienceAffector,
        HealthAffector,
        ManaAffector,
        StrengthAffector,
        None
    }
    public ItemEffect itemEffect;


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
            // instance = this;
            if (Inventory.instance != null)
            {
                Inventory.instance.AddItem(this);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"You picked up {itemName}!");
            }
        }
    }


    public bool GetIsStackable
    {
        get => isStackable;
    }
    public int GetMaxStackSize
    {
        get => maxStackSize;
    }

    public int GetCurrentStackSize
    {
        get => currentStackSize;
        set => currentStackSize = value;
    }
}
