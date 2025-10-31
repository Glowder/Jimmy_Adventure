using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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
    // FIXME: Start
    void Start()
    {
        CreateInstance();
        AmountPanelButtonFunctions();
        EquipmentUIElements();
        choosingAmountText.text = "0";
    }

    // FIXME: Update
    void Update()
    {

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
        if (setOrRemove == "Set" || setOrRemove == "set")
            equipmentUI[slot].GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite;

        if (setOrRemove == "remove" || setOrRemove == "Remove")
            equipmentUI[slot].GetComponentInChildren<UnityEngine.UI.Image>().sprite = MenuManager.instance.ItemFrame;
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

    // public void ChooseCharPanel(bool show)
    // {
    //     chooseCharPanel.SetActive(show);
    // }

    public bool ChooseCharPanel
    {
        get { return chooseCharPanel.activeSelf; }
        set { chooseCharPanel.SetActive(value); }
    }
    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
