using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

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
    [SerializeField] public UnityEngine.UI.Button[] unequipButtons = new UnityEngine.UI.Button[6];
    [SerializeField] GameObject choosingAmount, amountPanel, chooseCharPanel;
    // private int changedAmount;
    [SerializeField] TMP_InputField choosingAmountText;

    [SerializeField] UnityEngine.UI.Button reduceAmountButton, increaseAmountButton, confirmAmountButton, cancelAmountButton;
    [SerializeField] UnityEngine.UI.Button[] chooseCharButtons = new UnityEngine.UI.Button[6];

    private Dictionary<int, bool> initCheck = new Dictionary<int, bool>();

    private bool chooseCharPanelUpdate = false, allreadySet = false, mainMenuInitialized = false;

    // FIXME: Start
    void Start()
    {
        AmountPanelButtonFunctions(); //NOTE: initCheck.Key = 2
        ChooseCharPanelSetup(); //NOTE: initCheck.Key = 3
        choosingAmountText.text = "0";
        // AddArrayElementsDynamically(); //NOTE: initCheck.Key = 4
        AddListenerForButtons(); //NOTE: initCheck.Key = 5
    }

    // FIXME: Update
    void Update()
    {
        MethodInitializationLogic();
    }

    private void Awake()
    {
        CreateInstance(); //NOTE: initCheck.Key = 0
        EquipmentUIElements();  // NOTE: initCheck.Key = 1; Setting Key-Value pairs for equipment UI elements
    }

    // FIND: InitCheckDictionary
    private void InitCheckDictionary(int key, bool value, string set_Or_Change)
    {
        if (set_Or_Change.Equals("set", StringComparison.OrdinalIgnoreCase) && value == true)
        {
            Debug.LogError($"InitCheckDictionary ==> only false is allowed if you want to set.");
            Debug.LogError($"The value true is only allowed for change.");
            return;
        }
        // NOTE: just needs to set a key and its value. The order is not importnant and waht the key is does not matter
        if (set_Or_Change.Equals("set", StringComparison.OrdinalIgnoreCase))
        {
            if (!initCheck.ContainsKey(key))
            {
                initCheck.Add(key, value);
            }
            else
            {
                Debug.LogError($"InitCheckDictionary ==> key {key} already exists.");
            }
        }
        else if (set_Or_Change.Equals("change", StringComparison.OrdinalIgnoreCase))
        {
            if (initCheck.ContainsKey(key))
            {
                initCheck[key] = value;
            }
            else
            {
                Debug.LogError($"InitCheckDictionary ==> key {key} does not exist.");
            }
        }
        return;
    }

    private void MethodInitializationLogic() //NOTE: Calls various initialization methods if they haven't been called yet for some reason
    {
        if(MenuManager.instance != null && !MenuManager.instance.MenuCanvasActive && mainMenuInitialized)
        {
            mainMenuInitialized = false;
        }
        if(MenuManager.instance != null && MenuManager.instance.MenuCanvasActive && !mainMenuInitialized)
        {
            ChooseCharPanelSetup(); //NOTE: initCheck.Key = 3
            mainMenuInitialized = true;
        }
        if (allreadySet) return;
        int keyToCheck = -1;
        int[] dictKeys = initCheck.Keys.ToArray();
        if (!allreadySet)
        {
            for (int i = 0; i < dictKeys.Length; i++)
            {
                if (initCheck[dictKeys[i]] == false)
                {
                    keyToCheck = dictKeys[i];
                    break;
                }
            }
            if (keyToCheck == -1)
            {
                allreadySet = true;
            }
        }
        switch (keyToCheck)
        {
            case 0:
                CreateInstance();
                break;
            case 1:
                EquipmentUIElements();
                break;
            case 2:
                AmountPanelButtonFunctions();
                break;
            case 3:
                AddListenerForButtons();
                break;
            default:
                Debug.LogError("MethodInitializationLogic ==> No matching key found.");
                break;
        }
    }

    // private void AddArrayElementsDynamically()  //initCheck.Key = 4; This method can be used to dynamically populate arrays for any UI elements if needed
    // {
    //     if (!initCheck.ContainsKey(4))
    //         InitCheckDictionary(key: 4, value: false, set_Or_Change: "set");

    //     // string panelName = "Panel - ";
    //     string buttonName = "Button - ";

    //     if (instance == null)
    //     {
    //         Debug.LogError("ItemMenuManager => AddArrayElementsDynamically ==> instance is null!");
    //         return;
    //     }
    //     if (MenuManager.instance == null)
    //     {
    //         Debug.LogError("ItemMenuManager => AddArrayElementsDynamically ==> MenuManager.instance is null!");
    //         return;
    //     }

    //     if (instance != null && MenuManager.instance != null)
    //     {
    //         GameObject canvasObject = MenuManager.instance.MenuCanvas;
    //         List<UnityEngine.UI.Button> tempButton = new List<UnityEngine.UI.Button>();
    //         tempButton.AddRange(canvasObject.GetComponentsInChildren<UnityEngine.UI.Button>(true));
    //         Debug.Log($"======> Total buttons found in canvas: {tempButton.Count} <======");
    //         // if (unequipButtons2.Length < 0) return;
    //         int slotIndex = 0;
    //         string[] slots = equipmentUI.Keys.ToArray();
    //         UnityEngine.UI.Button[] neededButtons = new UnityEngine.UI.Button[slots.Length];
    //         for (int i = 0; i < neededButtons.Length; i++)
    //         {
    //             neededButtons[i] = null;
    //             Debug.Log($"neededButtons at index {i} set to null.");
    //         }
    //         for (int i = 0; i < tempButton.Count; i++)
    //         {
    //             Debug.Log("slotIndex is: " + slotIndex);
    //             if (slotIndex == slots.Length)
    //             {
    //                 Debug.Log($"slotIndex is {slotIndex} and slots.Length is {slots.Length}, breaking loop.");
    //                 break;
    //             }

    //             if (tempButton[i].name == buttonName + slots[slotIndex])
    //             {
    //                 Debug.Log($"slot value is {slots[i]}");
    //                 // Debug.Log($"ItemMenuManager => AddArrayElementsDynamically ==> Found button: {buttonName + slots[slotIndex]}");
    //                 neededButtons[slotIndex] = tempButton[i];
    //                 slotIndex++;
    //             }
    //         }
    //         for (int i = 0; i < slots.Length; i++)
    //         {
    //             Debug.Log($"Buttons name is =========> {neededButtons[i]?.name ?? "null"} <=========");
    //             // Debug.Log($"ItemMenuManager => AddArrayElementsDynamically ==> Finding button: {buttonName + slots[i]}");
    //             // unequipButtons2[i] = neededButtons[i];
    //         }
    //         InitCheckDictionary(key: 4, value: true, set_Or_Change: "change");
    //         return;
    //     }
    // }

    public void AddListenerForButtons() //NOTE: initCheck.Key = 5
    {
        InitCheckDictionary(key: 3, value: false, set_Or_Change: "set");
        if (instance != null)
        {
            string[] equipmentSlots = new string[] { "Weapon", "Head", "Shield", "Arms", "Chest", "Legs" };
            // NOTE: Gets the Keys and gives it to "slot" variable
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (unequipButtons != null && i < unequipButtons.Length && unequipButtons[i] != null)
                {
                    unequipButtons[i].onClick.RemoveAllListeners();
                }
                else
                {
                    Debug.LogWarning($"unequipButtons[{i}] is null or array is too short!");
                }
            }

            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (unequipButtons != null && i < unequipButtons.Length && unequipButtons[i] != null)
                {
                    string slot = equipmentSlots[i];
                    unequipButtons[i].onClick.AddListener(() => PlayerStats.instance.PlayerUnequipItem(slot));
                }
                else
                {
                    Debug.LogWarning($"unequipButtons[{i}] is null or array is too short!");
                }
            }
            InitCheckDictionary(key: 3, value: true, set_Or_Change: "change");
        }
        return;
    }
    private void EquipmentUIElements() //NOTE: initCheck.Key = 1
    {
        InitCheckDictionary(key: 1, value: false, set_Or_Change: "set");
        equipmentUI.Add("Head", equipmentHead);
        equipmentUI.Add("Chest", equipmentChest);
        equipmentUI.Add("Arms", equipmentArms);
        equipmentUI.Add("Legs", equipmentLegs);
        equipmentUI.Add("Weapon", equipmentWeapon);
        equipmentUI.Add("Shield", equipmentShield);
        InitCheckDictionary(key: 1, value: true, set_Or_Change: "change");
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

    private void AmountPanelButtonFunctions() //NOTE: initCheck.Key = 2
    {
        InitCheckDictionary(key: 2, value: false, set_Or_Change: "set");
        reduceAmountButton.onClick.AddListener(() => ChangeItemAmount(change: "decrease"));
        increaseAmountButton.onClick.AddListener(() => ChangeItemAmount(change: "increase"));
        confirmAmountButton.onClick.AddListener(() => ConfirmCancelItemAmount("confirm"));
        cancelAmountButton.onClick.AddListener(() => ConfirmCancelItemAmount("cancel"));
        InitCheckDictionary(key: 2, value: true, set_Or_Change: "change");
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

    public void ChooseCharPanelSetup() //NOTE: initCheck.Key = 3
    {
        #region "NullChecks"
        if (instance == null)
        {
            Debug.LogError("ItemMenuManager => ChooseCharPanelSetup ==> instance is null!");
            return;
        }
        if (GameManager.instance == null)
        {
            Debug.LogError("ItemMenuManager => ChooseCharPanelSetup ==> GameManager.instance is null!");
            return;
        }
        if (PlayerStats.instance == null)
        {
            Debug.LogError("ItemMenuManager => ChooseCharPanelSetup ==> PlayerStats.instance is null!");
            return;
        }
        if (MenuManager.instance == null)
        {
            Debug.LogError("ItemMenuManager => ChooseCharPanelSetup ==> MenuManager.instance is null!");
            return;
        }
        #endregion

        Debug.Log("ItemMenuManager => ChooseCharPanelSetup ==> Passed null checks.");
        if (GameManager.instance != null && PlayerStats.instance != null && MenuManager.instance != null)
        {
            // Check if the character selection panel is active and not yet updated
            if (MenuManager.instance.MenuCanvasActive && !chooseCharPanelUpdate)
            {
                Debug.Log("ItemMenuManager => ChooseCharPanelSetup ==> MenuCanvas is active and panel not updated.");
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
                Debug.Log("Character selection panel updated.");
                chooseCharPanelUpdate = true;
            }
            else if (MenuManager.instance != null && !MenuManager.instance.MenuCanvasActive && !chooseCharPanelUpdate)
            {
                Debug.Log("ItemMenuManager => ChooseCharPanelSetup ==> MenuCanvas is not active, no update needed.");
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

    private void CreateInstance() //NOTE: initCheck.Key = 0
    {
        InitCheckDictionary(key: 0, value: false, set_Or_Change: "set");
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        InitCheckDictionary(key: 0, value: true, set_Or_Change: "change");
    }

    #region "Getters and Setters"


    public bool ChooseCharPanel
    {
        get { return chooseCharPanel.activeSelf; }
        set { chooseCharPanel.SetActive(value); }
    }
    #endregion
}
