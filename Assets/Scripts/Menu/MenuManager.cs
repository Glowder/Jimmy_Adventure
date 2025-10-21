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

  [SerializeField] UnityEngine.UI.Button equipOrUseButton;
  [SerializeField] UnityEngine.UI.Button discardButton;
  [SerializeField] UnityEngine.UI.Button firstItemButton;

  // TODO: Remove the commented-out line. If itemButtonsList below works, this is no longer needed
  // [SerializeField] UnityEngine.UI.Button[] itemButtons = new UnityEngine.UI.Button[27];
  [SerializeField] List<UnityEngine.UI.Button> itemButtonsList = new List<UnityEngine.UI.Button>();
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
    CreateInstance();
    AddItemButtons(2);

    image.enabled = true;
    image = GetComponentInChildren<UnityEngine.UI.Image>();
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
        playerStats = GameManager.instance.GetPlayerStats();
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
    firstItemButton.onClick.RemoveAllListeners();
    firstItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
    for (int i = 0; i < buttonAmount; i++)
    {
      UnityEngine.UI.Button newButton = Instantiate(firstItemButton, firstItemButton.transform.parent);

      int buttonIndex = itemButtonsList.Count;
      // if (buttonIndex < 1) buttonIndex = 1;
      Debug.Log($"Button Index in AddItemButtons: {buttonIndex}");
      newButton.onClick.AddListener(() => Inventory.instance.ShowItemDetails(buttonIndex));
      newButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
      newButton.gameObject.SetActive(true);
      itemButtonsList.Add(newButton);
    }
  }

  public void UpdateStats()
  {
    if (playerStats == null || playerStats.Length == 0)
    {
      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetPlayerStats();
    }

    // The SetPlayerStatsArrayElementsInCanvas() method now handles positioning based on groupPositionNumber
    SetPlayerStatsArrayElementsInCanvas();
  }

  // Find and set all the UI elements in the menu canvas to their respective lists
  private void SetPlayerStatsArrayElementsInCanvas()
  {
    // Safety check
    if (playerStats == null || playerStats.Length == 0)
      return;

    // First, deactivate all slots
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
        int slotIndex = playerStats[i].GetGroupPositionNumber();

        // Make sure the slot index is valid
        if (slotIndex >= 0 && slotIndex < characterSlots.Length && characterSlots[slotIndex] != null)
        {
          characterSlots[slotIndex].SetActive(true);

          // Safe null checks for UI elements at the specific slot position
          if (nameText[slotIndex] != null) nameText[slotIndex].text = playerStats[i].GetPlayerName();
          if (healthText[slotIndex] != null) healthText[slotIndex].text = playerStats[i].GetHealth().ToString() + " / " + playerStats[i].GetMaxHealth().ToString();
          if (healthSlider[slotIndex] != null)
          {
            healthSlider[slotIndex].maxValue = playerStats[i].GetMaxHealth();
            healthSlider[slotIndex].value = playerStats[i].GetHealth();
          }
          if (manaText[slotIndex] != null) manaText[slotIndex].text = playerStats[i].GetMana().ToString() + " / " + playerStats[i].GetMaxMana().ToString();
          if (manaSlider[slotIndex] != null)
          {
            manaSlider[slotIndex].maxValue = playerStats[i].GetMaxMana();
            manaSlider[slotIndex].value = playerStats[i].GetMana();
          }
          if (expText[slotIndex] != null) expText[slotIndex].text = playerStats[i].GetExperience().ToString() + " / " + playerStats[i].GetMaxXP().ToString();
          if (expSlider[slotIndex] != null)
          {
            expSlider[slotIndex].maxValue = playerStats[i].GetMaxXP();
            expSlider[slotIndex].value = playerStats[i].GetExperience();
          }
          if (levelText[slotIndex] != null) levelText[slotIndex].text = "Level: " + playerStats[i].GetLevel().ToString();
          if (characterImages[slotIndex] != null) characterImages[slotIndex].sprite = playerStats[i].GetPlayerPortrait();
        }
        else
        {
          Debug.LogWarning($"Player {playerStats[i].GetPlayerName()} has invalid groupPositionNumber: {playerStats[i].GetGroupPositionNumber()}");
        }
      }
    }
  }

  public void UpdateInventoryUI(int inventarSlotIndex, int stackSize)
  {
    if (Inventory.instance.GetItemDetails(inventarSlotIndex) != null)
    {
      itemButtonsList[inventarSlotIndex].image.sprite = Inventory.instance.GetItemDetails(inventarSlotIndex).itemIcon;
      itemButtonsList[inventarSlotIndex].GetComponentInChildren<TextMeshProUGUI>().text = stackSize.ToString();
    }
  }

  private void ManageMenuCanvas()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {

      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetPlayerStats();

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
    image.GetComponent<Animator>().SetTrigger("Start Fade");
  }

  private void CreateInstance()
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

  public void SetEquipOrUseButtonText(string buttonText)
  {
    Debug.Log($"SetEquipOrUseButtonText called with argument: {buttonText}");

    // Null check for equipOrUseButton
    if (equipOrUseButton == null)
    {
      Debug.LogError("equipOrUseButton is null! Make sure it's assigned in the inspector.");
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
        Debug.LogError("TextMeshProUGUI component not found in equipOrUseButton children!");
        return;
      }

      textComponent.text = buttonText;
      Debug.Log($"Equip/Use button text set to: {textComponent.text}");
    }
  }

  #region Getter and Setter

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

  public int AvailableInventorySlots => itemButtonsList.Count;

  #endregion
}