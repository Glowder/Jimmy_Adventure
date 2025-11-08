using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
  public static MenuManager instance;

  private int selectedItemIndex, selectedCharacterIndex;
  [SerializeField] UnityEngine.UI.Button equipOrUseButton;
  [SerializeField] UnityEngine.UI.Button discardButton;
  [SerializeField] UnityEngine.UI.Button firstItemButton;
  [SerializeField] Sprite inventoryItemFrame;
  [SerializeField] public List<UnityEngine.UI.Button> itemButtonsList = new List<UnityEngine.UI.Button>();
  [SerializeField] UnityEngine.UI.Image image;
  [SerializeField] Animator animator;
  [SerializeField] GameObject menuCanvas;
  public PlayerStats[] playerStats;

  #region Stat Array UI Elements
  // Array to hold the UI slider elements for up to 6 characters
  [SerializeField]
  UnityEngine.UI.Slider[]
  healthSlider = new UnityEngine.UI.Slider[6],
  manaSlider = new UnityEngine.UI.Slider[6],
  expSlider = new UnityEngine.UI.Slider[6];

  // Array to hold the UI text elements for up to 6 characters
  [SerializeField]
  TextMeshProUGUI[]
  nameText = new TextMeshProUGUI[6],
  healthText = new TextMeshProUGUI[6],
  manaText = new TextMeshProUGUI[6],
  expText = new TextMeshProUGUI[6],
  levelText = new TextMeshProUGUI[6];
  [SerializeField] UnityEngine.UI.Image[] characterImages = new UnityEngine.UI.Image[6];
  // NOTE: Character Slots in the Stats Menu
  [SerializeField] GameObject[] characterSlots = new GameObject[6];
  #endregion

  #region Detailed Stats UI Elements
  [SerializeField] UnityEngine.UI.Image dSPortrait;
  [SerializeField] UnityEngine.UI.Slider dSHealthSlider, dSManaSlider, dSExpSlider;
  [SerializeField] TextMeshProUGUI dSHealthText, dSManaText, dSExpText, dSNameText, dSLevelText, dSStrengthText, dSIntelligenceText, dSCritText, dSDexterityText, dSPhysicalDEFText, dSMagicDEFText, dSPhysicalEvasion, dSMagicEvasion;
  #endregion

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // CreateInstance();
    AddListenerToButtons();

    image.enabled = true;
    image = GetComponentInChildren<UnityEngine.UI.Image>();
  }

  private void Awake()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {

    if (Input.GetKeyDown(KeyCode.U))
    {
      AddItemButtons(1);
    }

    if (playerStats == null || playerStats.Length == 0)
    {
      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetSortedPlayerStats();
    }

    // Only update UI elements if we have valid player stats
    if (playerStats != null && playerStats.Length > 0)
    {
      SetPlayerStatsArrayElementsInCanvas();
    }

    if (image == null)
    {
      image = GetComponentInChildren<UnityEngine.UI.Image>();
    }
    ManageMenuCanvas();

  }

  private void AddItemButtons(int buttonAmount)
  {
    int MAX_SIZE = 32;
    int MIN_SIZE = 8;
    if (itemButtonsList.Count >= MAX_SIZE)
    {
      Debug.Log("Maximum item buttons reached. Cannot add more.");
      return;
    }

    if (buttonAmount + itemButtonsList.Count > MAX_SIZE)
    {
      buttonAmount = MAX_SIZE - itemButtonsList.Count;
    }
    else if (buttonAmount + itemButtonsList.Count < MIN_SIZE)
    {
      buttonAmount = MIN_SIZE - itemButtonsList.Count;
    }

    firstItemButton.onClick.RemoveAllListeners();
    firstItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
    for (int i = 0; i < buttonAmount; i++)
    {
      UnityEngine.UI.Button newButton = Instantiate(firstItemButton, firstItemButton.transform.parent);

      int buttonIndex = itemButtonsList.Count;
      // Debug.Log($"Button Index in AddItemButtons: {buttonIndex}");
      newButton.onClick.AddListener(() => Inventory.instance.ShowItemDetails(buttonIndex));
      newButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
      newButton.gameObject.SetActive(true);
      itemButtonsList.Add(newButton);
    }
  }

  // Find and set all the UI elements in the menu canvas to their respective lists
  private void SetPlayerStatsArrayElementsInCanvas()
  {
    // Safety check
    if (playerStats == null || playerStats.Length == 0)
      return;

    // Deactivate all slots
    for (int i = 0; i < characterSlots.Length; i++)
    {
      if (characterSlots[i] != null)
        characterSlots[i].SetActive(false);
    }

    // Then activate and populate slots based on groupPositionNumber
    for (int i = 0; i < playerStats.Length; i++)
    {
      if (playerStats[i] != null)
      {
        // Get the group position (0-based for direct array indexing)
        int slotIndex = playerStats[i].PlayerGroupPositionNumber;

        // Make sure the slot index is valid
        if (slotIndex >= 0 && slotIndex < characterSlots.Length && characterSlots[slotIndex] != null)
        {
          characterSlots[slotIndex].SetActive(true);

          // Safe null checks for UI elements at the specific slot position
          if (nameText[slotIndex] != null) nameText[slotIndex].text = playerStats[i].PlayerName + " Ora Ora Ora";
          if (healthText[slotIndex] != null) healthText[slotIndex].text = playerStats[i].Health.ToString() + " / " + playerStats[i].MaxHealth.ToString();
          if (healthSlider[slotIndex] != null)
          {
            healthSlider[slotIndex].maxValue = playerStats[i].MaxHealth;
            healthSlider[slotIndex].value = playerStats[i].Health;
          }
          if (manaText[slotIndex] != null) manaText[slotIndex].text = playerStats[i].Mana.ToString() + " / " + playerStats[i].MaxMana.ToString();
          if (manaSlider[slotIndex] != null)
          {
            manaSlider[slotIndex].maxValue = playerStats[i].MaxMana;
            manaSlider[slotIndex].value = playerStats[i].Mana;
          }
          if (expText[slotIndex] != null) expText[slotIndex].text = playerStats[i].Experience.ToString() + " / " + playerStats[i].GetMaxXP().ToString();
          if (expSlider[slotIndex] != null)
          {
            expSlider[slotIndex].maxValue = playerStats[i].GetMaxXP();
            expSlider[slotIndex].value = playerStats[i].Experience;
          }
          if (levelText[slotIndex] != null) levelText[slotIndex].text = "Level: " + playerStats[i].Level.ToString();
          if (characterImages[slotIndex] != null) characterImages[slotIndex].sprite = playerStats[i].PlayerPortrait;
        }
        else
        {
          Debug.LogWarning($"Player {playerStats[i].PlayerName} has invalid groupPositionNumber: {playerStats[i].PlayerGroupPositionNumber}");
        }
      }
    }
  }

  public void UpdateInventoryUI(int inventorySlotIndex, int stackSize)
  {
    if (Inventory.instance.GetItemDetails(inventorySlotIndex) != null)
    {
      itemButtonsList[inventorySlotIndex].image.sprite = Inventory.instance.GetItemDetails(inventorySlotIndex).itemIcon;
      itemButtonsList[inventorySlotIndex].GetComponentInChildren<TextMeshProUGUI>().text = stackSize.ToString();
    }
  }

  public void ClearInventoryUI()
  {
    if (itemButtonsList == null)
      return;

    foreach (var itemButton in itemButtonsList)
    {
      var item = Inventory.instance.GetItemDetails(itemButtonsList.IndexOf(itemButton));
      if (item != null)
      {
        itemButton.image.sprite = item.itemIcon;
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.CurrentStackSize.ToString();
      }
      else
      {
        itemButton.image.sprite = inventoryItemFrame;
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
      }
    }
  }

  private void ManageMenuCanvas()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {

      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetSortedPlayerStats();

      if (!menuCanvas.activeInHierarchy && !DialogControl.instance.GetDialogBoxState())
      {
        menuCanvas.SetActive(true);
        if (Player.instance != null)
        {
          Player.instance.DeactivateMovementAndAnimation(deactivate: true);
          // Debug.Log("Menu open and player movement deactivated.");
        }
      }
      else if (menuCanvas.activeInHierarchy && !DialogControl.instance.GetDialogBoxState())
      {
        menuCanvas.SetActive(false);
        if (Player.instance != null)
        {
          Player.instance.DeactivateMovementAndAnimation(deactivate: false);
          // Debug.Log("Menu closed and player movement activated.");
        }
      }
    }
  }

  public void FadeImage()
  {
    if (image == null)
      image.GetComponent<Animator>().SetTrigger("Start Fade");
  }

  public void CreateInstance()
  {
    if (instance != null && instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      instance = this;
    }
  }

  public bool GetMenuCanvasState()
  {
    return menuCanvas.activeInHierarchy;
  }

  private void AddListenerToButtons()
  {
    AddItemButtons(16);
    discardButton.onClick.AddListener(() => DiscardItem());
  }

  // FIND: EquipOrUse
  public void EquipOrUse(int playerIndex)
  {
    if (Inventory.instance != null && instance != null && ItemMenuManager.instance != null)
    {
      int itemIndex = SelectedItemIndex;
      ItemManager item = Inventory.instance.GetItemDetails(itemIndex);
      ItemManager.ItemType consumable = ItemManager.ItemType.Consumable;
      ItemManager.ItemType equipment = ItemManager.ItemType.Equipment;
      Inventory inventory = Inventory.instance;

      if (item != null)
      {
        if (item.itemType == consumable)
        {
          inventory.UseConsumableItem(itemIndex: itemIndex, playerIndex: playerIndex);
        }
        else if (item.itemType == equipment)
        {
          inventory.EquipItemLogic(itemIndex: itemIndex, playerIndex: playerIndex);
        }
      }
    }
  }

  // FIND: DiscardItem
  public void DiscardItem()
  {
    Debug.Log($"Discarding item at index: {SelectedItemIndex}");
    ItemMenuManager.instance.ChoosingItemAmount(Inventory.instance.GetItemDetails(SelectedItemIndex));
  }
  public void SetEquipOrUseButtonText(string buttonText)
  {
    if (equipOrUseButton == null)
    {
      Debug.LogError("equipOrUseButton is null!");
      return;
    }

    if (buttonText == "HIDE")
    {
      equipOrUseButton.gameObject.SetActive(false);
    }
    else
    {
      equipOrUseButton.gameObject.SetActive(true);

      // Null check for TextMeshProUGUI component
      var textComponent = equipOrUseButton.GetComponentInChildren<TextMeshProUGUI>();
      if (textComponent == null)
      {
        return;
      }

      textComponent.text = buttonText;
    }
  }

  #region Getter and Setter

  public GameObject MenuCanvas => menuCanvas;

  public int SelectedCharacterIndex
  {
    get => selectedCharacterIndex;
    set => selectedCharacterIndex = value;
  }
  public bool MenuCanvasActive => menuCanvas.activeInHierarchy;

  public Sprite ItemFrame => inventoryItemFrame;

  // DS = Detailed Stats
  public UnityEngine.UI.Image DSPortrait
  {
    get => dSPortrait;
    set => dSPortrait = value;
  }

  public UnityEngine.UI.Slider DSHealthSlider
  {
    get => dSHealthSlider;
    set => dSHealthSlider = value;
  }

  public UnityEngine.UI.Slider DSManaSlider
  {
    get => dSManaSlider;
    set => dSManaSlider = value;
  }

  public UnityEngine.UI.Slider DSExpSlider
  {
    get => dSExpSlider;
    set => dSExpSlider = value;
  }

  public TextMeshProUGUI DSHealthText
  {
    get => dSHealthText;
    set => dSHealthText = value;
  }

  public TextMeshProUGUI DSManaText
  {
    get => dSManaText;
    set => dSManaText = value;
  }

  public TextMeshProUGUI DSExpText
  {
    get => dSExpText;
    set => dSExpText = value;
  }

  public TextMeshProUGUI DSNameText
  {
    get => dSNameText;
    set => dSNameText = value;
  }

  public TextMeshProUGUI DSLevelText
  {
    get => dSLevelText;
    set => dSLevelText = value;
  }

  public TextMeshProUGUI DSStrengthText
  {
    get => dSStrengthText;
    set => dSStrengthText = value;
  }

  public TextMeshProUGUI DSIntelligenceText
  {
    get => dSIntelligenceText;
    set => dSIntelligenceText = value;
  }

  public TextMeshProUGUI DSCritText
  {
    get => dSCritText;
    set => dSCritText = value;
  }

  public TextMeshProUGUI DSDexterityText
  {
    get => dSDexterityText;
    set => dSDexterityText = value;
  }

  public TextMeshProUGUI DSPhysicalDEFText
  {
    get => dSPhysicalDEFText;
    set => dSPhysicalDEFText = value;
  }

  public TextMeshProUGUI DSMagicDEFText
  {
    get => dSMagicDEFText;
    set => dSMagicDEFText = value;
  }

  public TextMeshProUGUI DSPhysicalEvasion
  {
    get => dSPhysicalEvasion;
    set => dSPhysicalEvasion = value;
  }

  public TextMeshProUGUI DSMagicEvasion
  {
    get => dSMagicEvasion;
    set => dSMagicEvasion = value;
  }

  public int SelectedItemIndex
  {
    get => selectedItemIndex;
    set => selectedItemIndex = value;
  }
  public int AvailableInventorySlots => itemButtonsList.Count;

  #endregion
}