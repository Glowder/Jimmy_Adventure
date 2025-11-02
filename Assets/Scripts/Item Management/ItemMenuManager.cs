using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class ItemMenuManager : MonoBehaviour
{
    public static ItemMenuManager instance;

    [SerializeField] public GameObject equipmentDetails;
    [SerializeField] public GameObject consumableDetails;
    [SerializeField] public TextMeshProUGUI itemCountConsume, itemNameConsume, itemDescriptionConsume, itemEffectDescriptionConsume, itemEffectConsume;
    [SerializeField] public GameObject miscellaneousDetails;
    [SerializeField] public TextMeshProUGUI itemCountMisc, itemNameMisc, itemDescriptionMisc;
    [SerializeField] public GameObject equipmentHead, equipmentChest, equipmentArms, equipmentLegs, equipmentWeapon, equipmentShield;
    [SerializeField] public Dictionary<string, GameObject> equipmentUI = new Dictionary<string, GameObject>();
    [SerializeField] GameObject choosingAmount, amountPanel, chooseCharPanel;
    // private int changedAmount;
    [SerializeField] TMP_InputField choosingAmountText;

    [SerializeField] UnityEngine.UI.Button reduceAmountButton, increaseAmountButton, confirmAmountButton, cancelAmountButton;
    [SerializeField] UnityEngine.UI.Button[] chooseCharButtons = new UnityEngine.UI.Button[6];

    private bool chooseCharPanelUpdate = false;

    // FIXME: Start
    void Start()
    {
        CreateInstance();
        AmountPanelButtonFunctions();
        EquipmentUIElements();
        ChooseCharPanelSetup();
        choosingAmountText.text = "0";
    }

    // FIXME: Update
    void Update()
    {
        ChooseCharPanelSetup();
    }

    private void EquipmentUIElements()
    {
        equipmentUI.Add("Head", equipmentHead);
        equipmentUI.Add("Chest", equipmentChest);
        equipmentUI.Add("Arms", equipmentArms);
        equipmentUI.Add("Legs", equipmentLegs);
        equipmentUI.Add("Weapon", equipmentWeapon);
        equipmentUI.Add("Shield", equipmentShield);
    }

    public void SetEquipmentUIElements(string setOrRemove, string slot, Sprite sprite)
    {
        if (MenuManager.instance != null && instance != null)
        {
            UnityEngine.UI.Image icon = equipmentUI[slot]?.GetComponentInChildren<UnityEngine.UI.Image>();

            if (setOrRemove.Equals("Set", StringComparison.OrdinalIgnoreCase))
                icon.sprite = sprite;

            if (setOrRemove.Equals("Remove", StringComparison.OrdinalIgnoreCase))
                icon.sprite = MenuManager.instance.ItemFrame;
        }
    }

    public void ChoosingItemAmount(ItemManager item)
    {
        if (Inventory.instance != null && MenuManager.instance != null && item != null)
        {
            ItemManager newItem = item;
            choosingAmountText.text = newItem.CurrentStackSize.ToString();
            Debug.Log("Choosing amount for item: " + newItem.itemName);
            Debug.Log("Current stack size: " + newItem.CurrentStackSize);
            choosingAmount.SetActive(true);
        }
    }

    private void AmountPanelButtonFunctions()
    {
        reduceAmountButton.onClick.AddListener(() => ChangeItemAmount(change: "decrease"));
        increaseAmountButton.onClick.AddListener(() => ChangeItemAmount(change: "increase"));
        confirmAmountButton.onClick.AddListener(() => ConfirmCancelItemAmount("confirm"));
        cancelAmountButton.onClick.AddListener(() => ConfirmCancelItemAmount("cancel"));
    }
    public void ChangeItemAmount(string change)
    {
        if (Inventory.instance != null && MenuManager.instance != null)
        {
            int itemIndex = MenuManager.instance.SelectedItemIndex;
            ItemManager item = Inventory.instance.GetItemDetails(itemIndex);
            int currentItemAmount = item.CurrentStackSize;

            if (item != null)
            {
                int currentAmount = int.Parse(choosingAmountText.text);
                if (change == "increase")
                {
                    if (currentAmount < item.maxStackSize)
                    {
                        choosingAmountText.text = (currentAmount + 1).ToString();
                        Debug.Log("Increased amount to: " + (currentAmount + 1));
                    }
                    else Debug.Log("Cannot increase amount beyond max stack size: " + item.maxStackSize);
                }
                else if (change == "decrease")
                {
                    if (currentAmount > 0)
                    {
                        choosingAmountText.text = (currentAmount - 1).ToString();
                        Debug.Log("Decreased amount to: " + (currentAmount - 1));
                    }
                    else Debug.Log("Cannot decrease amount below 0.");
                }
            }
            else Debug.Log("Select an Item first.");


        }
    }
    public void ConfirmCancelItemAmount(string action)
    {
        if (Inventory.instance != null && MenuManager.instance != null)
        {
            if (action == "confirm")
            {
                Debug.Log("Confirmed changing item amount to: " + choosingAmountText.text);
                choosingAmount.SetActive(false);
                int itemIndex = MenuManager.instance.SelectedItemIndex;
                ItemManager item = Inventory.instance.GetItemDetails(itemIndex);
                item.CurrentStackSize = int.Parse(choosingAmountText.text);
                Inventory.instance.UpdateInventoryUI(itemIndex, item.CurrentStackSize);
                if (item.CurrentStackSize == 0)
                {
                    Inventory.instance.RemoveItem(itemIndex);
                    MenuManager.instance.ClearInventoryUI();
                    Inventory.instance.ResetDetailsPanel("All");
                }
            }
            else if (action == "cancel")
            {
                Debug.Log("Cancelled changing item amount.");
                choosingAmount.SetActive(false);
            }
        }
    }

    public void ChooseCharPanelSetup()
    {
        if (GameManager.instance != null && PlayerStats.instance != null && MenuManager.instance != null)
        {
            // Check if the character selection panel is active and not yet updated
            if (MenuManager.instance.MenuCanvasActive && !chooseCharPanelUpdate)
            {
                PlayerStats[] playerStats = GameManager.instance.GetSortedPlayerStats();

                for (int i = 0; i < chooseCharButtons.Length - 1; i++)
                {
                    if (chooseCharButtons[i] == null)
                    {
                        Debug.LogError($"chooseCharButtons[{i}] is null!");
                        continue;
                    }
                    if (i < playerStats.Length && playerStats[i] != null)
                    {
                        chooseCharButtons[i].gameObject.SetActive(true);
                        chooseCharButtons[i].image.sprite = playerStats[i].PlayerPortrait;
                        chooseCharButtons[i].onClick.RemoveAllListeners();
                    }
                    else
                    {
                        chooseCharButtons[i].gameObject.SetActive(false);
                        Debug.Log($"No player stats for index {i}, hiding button.");
                    }
                }
                chooseCharPanelUpdate = true;
            }
            else if (MenuManager.instance != null && !MenuManager.instance.MenuCanvasActive && !chooseCharPanelUpdate)
            {
                return;
            }
            // NOTE: If the menu is not active, there is no need to update the panel
            else if (MenuManager.instance != null && !MenuManager.instance.MenuCanvasActive)
            {
                chooseCharPanelUpdate = false;
                Debug.Log("ChooseCharPanelSetup: " + chooseCharPanelUpdate);
            }
        }
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    #region "Getters and Setters"


    public bool ChooseCharPanel
    {
        get { return chooseCharPanel.activeSelf; }
        set { chooseCharPanel.SetActive(value); }
    }
    #endregion
}
