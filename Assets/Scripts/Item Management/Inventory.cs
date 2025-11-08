using System.Collections.Generic;
using System;
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
    public bool itemWasAdded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     ShowInventoryContents();
        // }
    }

    private void ShowInventoryContents()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                Debug.Log($"Inventory contains: {item.itemName} at position {items.IndexOf(item)}");
                count++;
            }
        }
        Debug.Log($"Total items in inventory: {count}. Free slots: {GetFreeSlotCount()}.");
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

        ResetDetailsPanel("All");
    }

    private List<ItemManager> CalculateStackamount(ItemManager item)
    {
        int totalStack = Mathf.CeilToInt((float)item.CurrentStackSize / item.GetMaxStackSize);
        int remainder = item.CurrentStackSize % item.GetMaxStackSize;
        List<ItemManager> dividedStacks = new List<ItemManager>();
        for (int i = 0; i < totalStack; i++)
        {
            ItemManager newItemStack = Instantiate(item);
            if (i == totalStack - 1 && remainder > 0)
            {
                newItemStack.CurrentStackSize = remainder;
            }
            else
            {
                newItemStack.CurrentStackSize = item.GetMaxStackSize;
            }
            dividedStacks.Add(newItemStack);
        }
        return dividedStacks;

    }

    private void AddStackableItem(ItemManager pickedUpItem)
    {
        stackableCheckedAndAdded = false;
        Debug.Log($"Picked up item stack size: {pickedUpItem.CurrentStackSize}.");
        for (int i = 0; i < items.Count; i++)
        {
            // NOTE: The item that was picked up is referenced by pickedUpItem
            Debug.Log($"Inventar item [i]: {items[i].itemName} with stack size: {items[i].CurrentStackSize}");
            if (items[i].itemID == pickedUpItem.itemID)
            {
                Debug.Log($"Found existing stack of {pickedUpItem.itemName} in inventory at slot {i + 1}. Stack size: {items[i].CurrentStackSize}");
                int tempStack = items[i].CurrentStackSize + pickedUpItem.CurrentStackSize;

                if (items[i].CurrentStackSize == items[i].GetMaxStackSize)
                {
                    Debug.Log($"{items[i].itemName} stack at slot {i + 1} is already at max size ({items[i].GetMaxStackSize}).");
                    continue;
                }
                else if (pickedUpItem.CurrentStackSize > pickedUpItem.GetMaxStackSize)
                {
                    items.AddRange(CalculateStackamount(pickedUpItem));
                    MenuManager.instance.ClearInventoryUI();
                    stackableCheckedAndAdded = true;
                    itemWasAdded = true;
                    return;
                }
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
                        UpdateInventoryUI(inventorySlot: i, stackSize: items[i].CurrentStackSize);
                        Debug.Log($"Stacked {tempStack} of {pickedUpItem.itemName}. New stack size: {items[i].CurrentStackSize}.");
                        stackableCheckedAndAdded = true;
                        itemWasAdded = true;
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
                            AddItemToNewSlot(item: pickedUpItem, stackSize: newStackSize);
                        }
                        UpdateInventoryUI(inventorySlot: i, stackSize: items[i].CurrentStackSize);
                        stackableCheckedAndAdded = true;
                        itemWasAdded = true;
                        Debug.Log($"===>>> Stack check, bigger than max size: {stackableCheckedAndAdded} <<<===");

                        return;
                    }
                }
            }
            else if (pickedUpItem.isStackable && pickedUpItem.CurrentStackSize > pickedUpItem.GetMaxStackSize)
            {
                items.AddRange(CalculateStackamount(pickedUpItem));
                MenuManager.instance.ClearInventoryUI();
                stackableCheckedAndAdded = true;
                itemWasAdded = true;
                return;
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
            UpdateInventoryUI(inventorySlot: items.Count - 1, stackSize: newItem.CurrentStackSize);
            itemWasAdded = true;
            //TEMP: this line is just temporary for testing
            Debug.Log($"Added {item.itemName} to empty position in Inventory with stack size: {newItem.CurrentStackSize}.");
            return;
        }
    }

    public void UpdateInventoryUI(int inventorySlot, int stackSize)
    {
        MenuManager.instance.UpdateInventoryUI(inventorySlotIndex: inventorySlot, stackSize: stackSize);
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
        if (slotIndex >= 0 && slotIndex < items.Count)
        {
            if (items[slotIndex].isStackable && items[slotIndex].CurrentStackSize > 1)
            {
                items[slotIndex].CurrentStackSize -= 1;
                UpdateInventoryUI(slotIndex, items[slotIndex].CurrentStackSize);
                return;
            }

            items.RemoveAt(slotIndex);
        }
    }

    // FIND: GetItemDetails
    public ItemManager GetItemDetails(int inventorySlotIndex)
    {
        if (items.Count > 0 && inventorySlotIndex >= 0 && inventorySlotIndex < items.Count)
        {
            if (items[inventorySlotIndex] != null)
            {
                return items[inventorySlotIndex];
            }
            else
            {
                return null;
            }
        }
        else
        {
            ResetDetailsPanel("All");
            return null;
        }
    }

    public void ShowItemDetails(int inventorySlot)
    {
        if (items.Count > 0)
        {
            ItemManager item = GetItemDetails(inventorySlot);


            if (item == null)
            {
                return;
            }

            if (MenuManager.instance != null && item != null)
            {
                MenuManager.instance.SelectedItemIndex = inventorySlot;
                if (item.itemType == ItemManager.ItemType.Consumable)
                    ItemDetailsConsumable(item);

                else if (item.itemType == ItemManager.ItemType.Equipment)
                    ItemDetailsEquipment(item);

                else if (item.itemType == ItemManager.ItemType.Miscellaneous)
                    ItemDetailsMiscellaneous(item);

                else
                    MenuManager.instance.SetEquipOrUseButtonText("HIDE");

            }
        }
        else
        {
            Debug.Log("Inventory is empty.");
        }
    }

    private void ItemDetailsEquipment(ItemManager item)
    {
        if (MenuManager.instance != null && ItemMenuManager.instance != null)
        {
            ItemMenuManager details = ItemMenuManager.instance;
            ResetDetailsPanel("Equipment");
            details.equipmentDetails.SetActive(true);
            details.consumableDetails.SetActive(false);
            details.miscellaneousDetails.SetActive(false);
            MenuManager.instance.SetEquipOrUseButtonText("EQUIP");

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

    private void ItemDetailsConsumable(ItemManager item)
    {
        if (MenuManager.instance != null && ItemMenuManager.instance != null)
        {
            ItemMenuManager details = ItemMenuManager.instance;
            ResetDetailsPanel("Consumable");
            details.equipmentDetails.SetActive(false);
            details.consumableDetails.SetActive(true);
            details.miscellaneousDetails.SetActive(false);
            MenuManager.instance.SetEquipOrUseButtonText("USE");
            details.itemCountConsume.text = item.CurrentStackSize.ToString() + " / " + item.GetMaxStackSize.ToString();
            details.itemNameConsume.text = item.itemName;
            details.itemDescriptionConsume.text = item.itemDescription;
            details.itemEffectDescriptionConsume.text = item.itemEffectDescription;
            if (item.itemHPBoost > 0 && item.itemMPBoost == 0)
            {
                details.itemEffectConsume.text = $"Heals {item.itemHPBoost} HP.";
            }
            else if (item.itemMPBoost > 0 && item.itemHPBoost == 0)
            {
                details.itemEffectConsume.text = $"Heals {item.itemMPBoost} MP.";
            }
            else if (item.itemHPBoost > 0 && item.itemMPBoost > 0)
            {
                details.itemEffectConsume.text = $"Heals {item.itemHPBoost} HP and {item.itemMPBoost} MP.";
            }
            else
            {
                details.itemEffectConsume.text = "No effect.";
            }
        }
    }

    private void ItemDetailsMiscellaneous(ItemManager item)
    {
        if (MenuManager.instance != null && ItemMenuManager.instance != null)
        {
            ItemMenuManager details = ItemMenuManager.instance;
            ResetDetailsPanel("Miscellaneous");
            details.equipmentDetails.SetActive(false);
            details.consumableDetails.SetActive(false);
            details.miscellaneousDetails.SetActive(true);
            MenuManager.instance.SetEquipOrUseButtonText("HIDE");

            // Set miscellaneous item data
            details.itemCountMisc.text = item.CurrentStackSize.ToString();
            details.itemNameMisc.text = item.itemName;
            details.itemDescriptionMisc.text = item.itemDescription;
        }
    }

    public void ResetDetailsPanel(string itemType)
    {
        ItemMenuManager details = ItemMenuManager.instance;

        if (itemType == "Equipment" || itemType == "All")
        {
            // Consumable
            details.itemCountConsume.text = "";
            details.itemNameConsume.text = "";
            details.itemDescriptionConsume.text = "";
            details.itemEffectDescriptionConsume.text = "";
            details.itemEffectConsume.text = "";
            // Miscellaneous
            details.itemCountMisc.text = "0";
            details.itemNameMisc.text = "";
            details.itemDescriptionMisc.text = "";
        }
        if (itemType == "Consumable" || itemType == "All")
        {
            // Equipment
            itemDetailsStackSize.text = "";
            itemDetailsName.text = "";
            itemDetailsDescription.text = "";
            itemDetailHP.text = "";
            itemDetailMP.text = "";
            itemDetailStrength.text = "";
            itemDetailIntelligence.text = "";
            itemDetailSpecialAttribute.text = "";
            itemDetailCrit.text = "";
            itemDetailPhysicalDEF.text = "";
            itemDetailMagicDEF.text = "";
            // Miscellaneous
            details.itemCountMisc.text = "";
            details.itemNameMisc.text = "";
            details.itemDescriptionMisc.text = "";
        }
        if (itemType == "Miscellaneous" || itemType == "All")
        {
            // Equipment
            itemDetailsStackSize.text = "";
            itemDetailsName.text = "";
            itemDetailsDescription.text = "";
            itemDetailHP.text = "";
            itemDetailMP.text = "";
            itemDetailStrength.text = "";
            itemDetailIntelligence.text = "";
            itemDetailSpecialAttribute.text = "";
            itemDetailCrit.text = "";
            itemDetailPhysicalDEF.text = "";
            itemDetailMagicDEF.text = "";
            // Consumable
            details.itemCountConsume.text = "";
            details.itemNameConsume.text = "";
            details.itemDescriptionConsume.text = "";
            details.itemEffectDescriptionConsume.text = "";
            details.itemEffectConsume.text = "";
        }
    }

    public void UseConsumableItem(int itemIndex, int playerIndex)
    {
        ItemManager item = GetItemDetails(itemIndex);
        ItemManager.ItemType consumable = ItemManager.ItemType.Consumable;
        PlayerStats[] player = GameManager.instance.GetSortedPlayerStats();

        if (item != null)
        {
            if (item.CurrentStackSize > 0 && item.itemType == consumable)
            {

                int tempHealth = player[playerIndex].Health + item.itemHPBoost;
                int tempMana = player[playerIndex].Mana + item.itemMPBoost;

                // Note: Added handling for items that boost only HP
                if (item.itemHPBoost > 0 && item.itemMPBoost == 0)
                {
                    if (player[playerIndex].Health >= player[playerIndex].MaxHealth)
                    {
                        Debug.Log("Player health is already at maximum.");
                        return;
                    }
                    if (tempHealth > player[playerIndex].MaxHealth)
                    {
                        player[playerIndex].Health = player[playerIndex].MaxHealth;
                        RemoveItem(itemIndex);
                        MenuManager.instance.ClearInventoryUI();
                    }
                    else
                    {
                        player[playerIndex].Health = tempHealth;
                        RemoveItem(itemIndex);
                        MenuManager.instance.ClearInventoryUI();
                    }
                }
                // Note: Added handling for items that boost only MP
                else if (item.itemMPBoost > 0 && item.itemHPBoost == 0)
                {
                    if (player[playerIndex].Mana >= player[playerIndex].MaxMana)
                    {
                        Debug.Log("Player mana is already at maximum.");
                        return;
                    }
                    if (tempMana > player[playerIndex].MaxMana)
                    {
                        player[playerIndex].Mana = player[playerIndex].MaxMana;
                        RemoveItem(itemIndex);
                        MenuManager.instance.ClearInventoryUI();
                    }
                    else
                    {
                        player[playerIndex].Mana = tempMana;
                        RemoveItem(itemIndex);
                        MenuManager.instance.ClearInventoryUI();
                    }
                }
                // Note: Added handling for items that boost both HP and MP
                else if (item.itemHPBoost > 0 && item.itemMPBoost > 0)
                {
                    if (player[playerIndex].Health >= player[playerIndex].MaxHealth && player[playerIndex].Mana >= player[playerIndex].MaxMana)
                    {
                        Debug.Log("Player health and mana are already at maximum.");
                        return;
                    }
                    // Note: First handle HP
                    if (tempHealth > player[playerIndex].MaxHealth)
                    {
                        player[playerIndex].Health = player[playerIndex].MaxHealth;
                    }
                    else
                    {
                        player[playerIndex].Health = tempHealth;
                    }
                    // Note: Then handle MP
                    if (tempMana > player[playerIndex].MaxMana)
                    {
                        player[playerIndex].Mana = player[playerIndex].MaxMana;
                    }
                    else
                    {
                        player[playerIndex].Mana = tempMana;
                    }
                    RemoveItem(itemIndex);
                    MenuManager.instance.ClearInventoryUI();
                }
            }
        }
    }

    // FIND: EquipItem
    public void EquipItemLogic(int itemIndex, int playerIndex)
    {
        #region "Initial Definitions"
        ItemManager item = GetItemDetails(itemIndex);
        ItemManager.ItemType equipment = ItemManager.ItemType.Equipment;
        // Note: Define equipment types
        ItemManager.EquipmentType weapon = ItemManager.EquipmentType.Weapon;
        ItemManager.EquipmentType shield = ItemManager.EquipmentType.Shield;
        ItemManager.EquipmentType head = ItemManager.EquipmentType.Head;
        ItemManager.EquipmentType chest = ItemManager.EquipmentType.Chest;
        ItemManager.EquipmentType arms = ItemManager.EquipmentType.Arms;
        ItemManager.EquipmentType legs = ItemManager.EquipmentType.Legs;
        // Note: Define equipment slots
        ItemManager.EquipSlotNeeded oneHanded = ItemManager.EquipSlotNeeded.OneHanded;
        ItemManager.EquipSlotNeeded twoHanded = ItemManager.EquipSlotNeeded.TwoHanded;

        ItemMenuManager equipmentUI = ItemMenuManager.instance;

        PlayerStats[] player = GameManager.instance.GetSortedPlayerStats();
        #endregion

        if (item != null && player != null && playerIndex >= 0 && playerIndex < player.Length)
        {
            if (item.CurrentStackSize > 0 && item.itemType == equipment)
            {
                // Note: Equip logic goes here
                player[playerIndex].MaxHealth += item.itemHPBoost;
                player[playerIndex].MaxMana += item.itemMPBoost;
                player[playerIndex].Strength += item.itemStrengthBoost;
                player[playerIndex].CritChance += item.itemCritBoost;
                player[playerIndex].PhysicalDEF += item.itemPhysicalDEFBoost;
                player[playerIndex].Intelligence += item.itemIntelligenceBoost;
                player[playerIndex].MagicalDEF += item.itemMagicDEFBoost;

                if (item.equipmentType == weapon)
                {
                    if (item.equipSlotNeeded == oneHanded)
                    {
                        player[playerIndex].PlayerEquipItem("Weapon", item);
                    }
                    else if (item.equipSlotNeeded == twoHanded)
                    {
                        player[playerIndex].PlayerEquipItem("Weapon", item);
                        player[playerIndex].PlayerEquipItem("Shield", item);
                    }
                }
                else if (item.equipmentType == shield)
                {
                    player[playerIndex].PlayerEquipItem("Shield", item);
                }
                else if (item.equipmentType == head)
                {
                    player[playerIndex].PlayerEquipItem("Head", item);
                }
                else if (item.equipmentType == chest)
                {
                    player[playerIndex].PlayerEquipItem("Chest", item);
                }
                else if (item.equipmentType == arms)
                {
                    player[playerIndex].PlayerEquipItem("Arms", item);
                }
                else if (item.equipmentType == legs)
                {
                    player[playerIndex].PlayerEquipItem("Legs", item);
                }

                RemoveItem(itemIndex);
                MenuManager.instance.ClearInventoryUI();

                return;
            }
        }
    }

    public int GetInventoryItemCount => items.Count;
}
