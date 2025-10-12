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

  [SerializeField] UnityEngine.UI.Button button;
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

    image.enabled = true;
    image = GetComponentInChildren<UnityEngine.UI.Image>();

    // for (int i = 0; i < nameText.Length; i++)
    // {
    //   GameObject placeholder = new GameObject("Name Text placeholder " + i);
    //   placeholder.transform.SetParent(this.transform);
    //   nameText[i] = placeholder.AddComponent<TextMeshProUGUI>();
    //   nameText[i].text = "Dis Name is " + i;
    // }
  }

  // Update is called once per frame
  void Update()
  {
    // button.onClick.AddListener(UpdateStats);

    SetPlayerStatsArrayElementsInCanvas();
    if (Input.GetKeyDown(KeyCode.U))
    {
      // UpdateStats();
    }

    if (playerStats == null || playerStats.Length == 0)
    {
      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetPlayerStats();
    }

    if (image == null)
    {
      image = GetComponentInChildren<UnityEngine.UI.Image>();
    }
    ManageMenuCanvas();

  }

  public void UpdateStats()
  {

    if (playerStats == null || playerStats.Length == 0)
    {
      if (GameManager.instance != null)
        playerStats = GameManager.instance.GetPlayerStats();
    }

    SetPlayerStatsArrayElementsInCanvas();

    foreach (GameObject slot in characterSlots)
      slot.SetActive(false);

    for (int i = 0; i < playerStats.Length - 1; i++)
    {
      nameText[i].text = playerStats[i].GetPlayerName();
      healthText[i].text = playerStats[i].GetHealth().ToString() + " / " + playerStats[i].GetMaxHealth().ToString();
      healthSlider[i].maxValue = playerStats[i].GetMaxHealth();
      healthSlider[i].value = playerStats[i].GetHealth();
      manaText[i].text = playerStats[i].GetMana().ToString() + " / " + playerStats[i].GetMaxMana().ToString();
      manaSlider[i].maxValue = playerStats[i].GetMaxMana();
      manaSlider[i].value = playerStats[i].GetMana();
      expText[i].text = playerStats[i].GetExperience().ToString() + " / " + playerStats[i].GetMaxXP().ToString();
      expSlider[i].maxValue = playerStats[i].GetMaxXP();
      expSlider[i].value = playerStats[i].GetExperience();
      levelText[i].text = "Level: " + playerStats[i].GetLevel().ToString();
      characterImages[i].sprite = playerStats[i].GetPlayerPortrait();
    }
  }

  // Find and set all the UI elements in the menu canvas to their respective lists
  private void SetPlayerStatsArrayElementsInCanvas()
  {
    for (int i = 0; i < playerStats.Length; i++)
    {
      if (i >= playerStats.Length)
      {
        characterSlots[i].SetActive(false);

      }
      else
      {
        characterSlots[i].SetActive(true);
        nameText[i].text = playerStats[i].GetPlayerName();
        healthText[i].text = playerStats[i].GetHealth().ToString() + " / " + playerStats[i].GetMaxHealth().ToString();
        healthSlider[i].maxValue = playerStats[i].GetMaxHealth();
        healthSlider[i].value = playerStats[i].GetHealth();
        manaText[i].text = playerStats[i].GetMana().ToString() + " / " + playerStats[i].GetMaxMana().ToString();
        manaSlider[i].maxValue = playerStats[i].GetMaxMana();
        manaSlider[i].value = playerStats[i].GetMana();
        // expText[i].text = playerStats[i].GetExperience().ToString() + " / " + playerStats[i].GetMaxXP().ToString();
        expSlider[i].maxValue = playerStats[i].GetMaxXP();
        expSlider[i].value = playerStats[i].GetExperience();
        levelText[i].text = "Level: " + playerStats[i].GetLevel().ToString();
        characterImages[i].sprite = playerStats[i].GetPlayerPortrait();
      }
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
          Debug.Log("Menu open and player movement deactivated.");
        }
      }
      else if (menuCanvas.activeInHierarchy && !DialogControl.instance.GetDialogBoxState())
      {
        menuCanvas.SetActive(false);
        if (Player.instance != null)
        {
          Player.instance.DeactivateMovementAndAnimation(deactivate: false);
          Debug.Log("Menu closed and player movement activated.");
        }
      }

      // UpdateStats();
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

  #region Getter and Setter

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

  #endregion
}
