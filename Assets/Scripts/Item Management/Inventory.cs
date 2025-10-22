using System.Collections.Generic;
using System.Threading;
using UnityEditor.Search;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private List<ItemManager> items = new List<ItemManager>();
    public enum InventoryType { PlayerInventory, ChestInventory, ShopInventory }
    public InventoryType inventoryType;

    [SerializeField]
    TMPro.TextMeshProUGUI itemDetailsStackSize, itemDetailsName, itemDetailsDescription,
    itemDetailHP, itemDetailMP, itemDetailStrength, itemDetailIntelligence,
    itemDetailSpecialAttribute, itemDetailCrit, itemDetailPhysicalDEF, itemDetailMagicDEF;

    private bool stackableCheckedAndAdded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        //TEMP: this line is just temporary for testing
        //  Debug.Log($"Inventory instance is {instance.enabled} and {items.Length} Items were created.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventoryContents();
        }
    }

    private void ShowInventoryContents()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                //TEMP: this line is just temporary for testing
                Debug.Log($"Inventory contains: {item.itemName}");
                count++;
            }
        }
        //TEMP: this line is just temporary for testing
        // Debug.Log($"Total items in inventory: {count}. \nFree slots: {GetFreeSlotCount()}.");
    }

    public void AddItem(ItemManager item)
    {
        Debug.Log($"===>>> Stack check, at AddItem: {stackableCheckedAndAdded} <<<===");
        if (items.Count == MenuManager.instance.AvailableInventorySlots)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        if (item.isStackable)
        {
            AddStackableItem(item);

            if (!stackableCheckedAndAdded)
            {
                AddItemToNewSlot(item, item.CurrentStackSize);
            }
        }
        else
        {
            // Non-stackable items go directly to new slot
            AddItemToNewSlot(item, item.CurrentStackSize);
        }
    }
    private void AddStackableItem(ItemManager pickedUpItem)
    {
        stackableCheckedAndAdded = false;
        Debug.Log($"Picked up item stack size: {pickedUpItem.CurrentStackSize}.");
        // Debug.Log($"===>>> Stack check, at the start: {stackableCheckedAndAdded} <<<===");
        for (int i = 0; i < items.Count; i++)
        {
            // NOTE: The item that was picked up is referenced by pickedUpItem
            Debug.Log($"Inventar item [i]: {items[i].itemName} \nwith stack size: {items[i].CurrentStackSize}");
            if (items[i].itemID == pickedUpItem.itemID)
            {
                Debug.Log($"Found existing stack of {pickedUpItem.itemName} in inventory at slot {i + 1}. Stack size: {items[i].CurrentStackSize}");
                int tempStack = items[i].CurrentStackSize + pickedUpItem.CurrentStackSize;
                if (items[i].CurrentStackSize == items[i].GetMaxStackSize)
                {
                    Debug.Log($"{items[i].itemName} stack at slot {i + 1} is already at max size ({items[i].GetMaxStackSize}).");
                    continue;
                }
                // else if (tempStack != inventarItem.GetMaxStackSize)
                else
                {
                    Debug.Log($"Current stack size before adding: {items[i].CurrentStackSize}");
                    // int tempStack = inventarItem.CurrentStackSize + pickedUpItem.CurrentStackSize;
                    Debug.Log($"Temp stack size after adding: {tempStack}");
                    if (tempStack < items[i].GetMaxStackSize)
                    {
                        Debug.Log("Item stack is smaller than max stack size.");
                        items[i].CurrentStackSize = tempStack;
                        Debug.Log($"Inventar item stack size: {items[i].CurrentStackSize}");
                        UpdateInventoryUI(inventarSlot: i, stackSize: items[i].CurrentStackSize);
                        Debug.Log($"Stacked {tempStack} of {pickedUpItem.itemName}. New stack size: {items[i].CurrentStackSize}.");
                        stackableCheckedAndAdded = true;
                        Debug.Log($"===>>> Stack check, smaller than max size: {stackableCheckedAndAdded} <<<===");
                        return;
                    }
                    else
                    {

                        items[i].CurrentStackSize = items[i].GetMaxStackSize;
                        Debug.Log("Item stack exceeds max stack size.");
                        int newStackSize = tempStack - items[i].GetMaxStackSize;
                        Debug.Log($"Stack size: {tempStack}. NewStack: {newStackSize}");
                        if (newStackSize != 0)
                        {
                            AddItemToNewSlot(pickedUpItem, newStackSize);
                        }
                        UpdateInventoryUI(inventarSlot: i, stackSize: items[i].CurrentStackSize);
                        stackableCheckedAndAdded = true;
                        Debug.Log($"===>>> Stack check, bigger than max size: {stackableCheckedAndAdded} <<<===");

                        return;
                    }
                }
            }
            else stackableCheckedAndAdded = false;
        }
    }

    private void AddItemToNewSlot(ItemManager item, int stackSize)
    {
        if (items.Count < MenuManager.instance.AvailableInventorySlots)
        {
            ItemManager newItem = Instantiate(item);
            newItem.CurrentStackSize = stackSize;
            items.Add(newItem);
            UpdateInventoryUI(inventarSlot: items.Count - 1, stackSize: newItem.CurrentStackSize);
            //TEMP: this line is just temporary for testing
            Debug.Log($"Added {item.itemName} to empty position in Inventory with stack size: {newItem.CurrentStackSize}.");
            return;
        }
    }

    private void UpdateInventoryUI(int inventarSlot, int stackSize)
    {
        MenuManager.instance.UpdateInventoryUI(inventarSlotIndex: inventarSlot, stackSize: stackSize);
    }

    private int GetItemCount()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item != null) count++;
        }
        return count;
    }
    public int GetFreeSlotCount()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item == null) count++;
        }
        return count;
    }

    public void RemoveItem(int slotIndex)
    {

    }

    public ItemManager GetItemDetails(int inventarSlotIndex)
    {
        if (items.Count > 0)
        {
            if (items[inventarSlotIndex] != null)
            {
                return items[inventarSlotIndex];
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.Log("Inventory slot is empty.");
            return null;
        }
    }

    public void ShowItemDetails(int inventarSlot)
    {
        if (items.Count > 0)
        {
            ItemManager item = GetItemDetails(inventarSlot);


            if (item == null)
            {
                return;
            }

            if (MenuManager.instance != null && item != null)
            {
                if (item.itemType == ItemManager.ItemType.Consumable)
                    MenuManager.instance.SetEquipOrUseButtonText("USE");

                else if (item.itemType == ItemManager.ItemType.Equipment)
                    MenuManager.instance.SetEquipOrUseButtonText("EQUIP");

                else
                    MenuManager.instance.SetEquipOrUseButtonText("HIDE");

                itemDetailsStackSize.text = item.CurrentStackSize.ToString();
                itemDetailsName.text = item.itemName;
                itemDetailsDescription.text = item.itemDescription;
                itemDetailHP.text = "HP: " + item.itemHPBoost.ToString();
                itemDetailMP.text = "MP: " + item.itemMPBoost.ToString();
                itemDetailStrength.text = "Strength: " + item.itemStrengthBoost.ToString();
                itemDetailIntelligence.text = "Intelligence: " + item.itemIntelligenceBoost.ToString();
                itemDetailSpecialAttribute.text = "NONE!";
                itemDetailCrit.text = "Crit: " + item.itemCritBoost.ToString();
                itemDetailPhysicalDEF.text = "Physical DEF: " + item.itemPhysicalDEFBoost.ToString();
                itemDetailMagicDEF.text = "Magic DEF: " + item.itemMagicDEFBoost.ToString();

            }
        }
        else
        {
            Debug.Log("Inventory is empty.");
        }
    }

    public int GetInventoryItemCount => items.Count;
}
