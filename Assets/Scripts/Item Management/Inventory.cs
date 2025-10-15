using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private ItemManager[] items = new ItemManager[27];
    public enum InventoryType { PlayerInventory, ChestInventory, ShopInventory }
    public InventoryType inventoryType;

    public bool[] isInvSlotFree = new bool[27];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        Debug.Log($"Inventory instance is {instance.enabled} and {items.Length} Items were created.");
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
        foreach (var item in items)
        {
            Debug.Log($"Inventory contains: {item.itemName}");
        }
    }

    public void AddItem(ItemManager item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null && items[i] != item)
            {
                if (!item.GetIsStackable)
                    item.GetCurrentStackSize = 1;

                items[i] = item;
                MenuManager.instance.UpdateInventoryUI(i);
                Debug.Log($"Added {item.itemName} to Inventory. Inventory now has {GetItemCount()} items.");
                return;
            }
            else if (items[i] == item && item.GetIsStackable && item.GetCurrentStackSize < item.GetMaxStackSize)
            {
                item.GetCurrentStackSize++;
                Debug.Log($"Increased stack size of {item.itemName} to {item.GetCurrentStackSize}");
                return;
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

    public void RemoveItem(int slotIndex)
    {

    }

    public ItemManager GetItemDetails(int slotIndex)
    {
        if (items[slotIndex] != null)
        {
            // Debug.Log($"Item Details: Name: {items[slotIndex].itemName}, Description: {items[slotIndex].itemDescription}, Type: {items[slotIndex].itemType}");
            return items[slotIndex];
        }
        else
        {
            Debug.Log("No item in this slot.");
            return null;
        }
    }
}
