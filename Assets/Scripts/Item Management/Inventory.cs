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

    public bool[] isInvSlotFree = new bool[27];

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
        for (int i = 0; i < MenuManager.instance.GetItemButtonCount; i++)
        {
            if (items[i] == null)
            {
                item.CurrentStackSize = 1;
                items[i] = item;
                MenuManager.instance.UpdateInventoryUI(i);
                //TEMP: this line is just temporary for testing
                // Debug.Log($"Added {item.itemName} to Inventory. Inventory now has {GetItemCount()} items.");
                return;  // Exit method after successfully adding item
            }
            else if (items[i] == item && item.GetIsStackable && item.CurrentStackSize < item.GetMaxStackSize)
            {
                item.CurrentStackSize++;
                //TEMP: this line is just temporary for testing
                // Debug.Log($"Increased stack size of {item.itemName} to {item.GetCurrentStackSize}");
                MenuManager.instance.UpdateInventoryUI(i);  // Also update UI for stack increase
                return;  // Exit method after successfully stacking item
            }
        }
        Debug.Log("Inventory is full!");
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

    public ItemManager GetItemDetails(int slotIndex)
    {
        if (items[slotIndex] != null)
        {
            //TEMP: this line is just temporary for testing
            // Debug.Log($"Item Details: Name: {items[slotIndex].itemName}, Description: {items[slotIndex].itemDescription}, Type: {items[slotIndex].itemType}");
            return items[slotIndex];
        }
        else
        {
            //TEMP: this line is just temporary for testing
            Debug.Log("No item in this slot.");
            return null;
        }
    }
}
