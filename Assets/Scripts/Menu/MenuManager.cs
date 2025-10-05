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
  [SerializeField] UnityEngine.UI.Image image;
  [SerializeField] Animator animator;
  [SerializeField] GameObject menuCanvas;
  public PlayerStats[] playerStats;
  [SerializeField] List<UnityEngine.UI.Slider>
  healthSlider = new List<UnityEngine.UI.Slider>(),
  manaSlider = new List<UnityEngine.UI.Slider>(),
  expSlider = new List<UnityEngine.UI.Slider>();

  [SerializeField]
  List<TextMeshProUGUI> nameText = new List<TextMeshProUGUI>(),
  healthText = new List<TextMeshProUGUI>(),
  manaText = new List<TextMeshProUGUI>(),
  expText = new List<TextMeshProUGUI>(),
  levelText = new List<TextMeshProUGUI>(),
  dexterityText = new List<TextMeshProUGUI>(),
  physicalDEFText = new List<TextMeshProUGUI>(),
  magicalDEFText = new List<TextMeshProUGUI>(),
  strengthText = new List<TextMeshProUGUI>(),
  intelligenceText = new List<TextMeshProUGUI>();

  [SerializeField] List<UnityEngine.UI.Image> characterImages = new List<UnityEngine.UI.Image>();
  [SerializeField] List<GameObject> characterSlots = new List<GameObject>();

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();

    image.enabled = true;
    image = GetComponentInChildren<UnityEngine.UI.Image>();
  }

  // Update is called once per frame
  void Update()
  {

    if (Input.GetKeyDown(KeyCode.U))
    {
      // UpdateStats();
      SetPlayerStatsArrayElementsInCanvas();
    }

    if (image == null)
    {
      image = GetComponentInChildren<UnityEngine.UI.Image>();
    }
    ManageMenuCanvas();

  }

  public void UpdateStats()
  {
    // SetPlayerStatsArrayElementsInCanvas();

    // foreach (GameObject slot in characterSlots)
    //   slot.SetActive(false);

    // for (int i = 0; i < playerStats.Length; i++)
    // {
    //   characterSlots[i].SetActive(true);
    //   nameText[i].text = playerStats[i].GetPlayerName();
    //   characterImages[i].sprite = playerStats[i].GetPlayerPortrait();
    //   levelText[i].text = playerStats[i].GetLevel().ToString();
    //   healthText[i].text = playerStats[i].GetHealth().ToString() + " / " + playerStats[i].GetMaxHealth().ToString();
    //   manaText[i].text = playerStats[i].GetMana().ToString() + " / " + playerStats[i].GetMaxMana().ToString();
    //   expText[i].text = playerStats[i].GetExperience().ToString() + " / " + playerStats[i].GetMaxXP().ToString();
    // }


  }

  // Find and set all the UI elements in the menu canvas to their respective lists
  private void SetPlayerStatsArrayElementsInCanvas()
  {

    healthSlider.Clear();
    healthSlider.AddRange(
        menuCanvas.GetComponentsInChildren<UnityEngine.UI.Slider>(true)
            .Where(go => go.CompareTag("HP Slider"))
    );

    manaSlider.Clear();
    manaSlider.AddRange(
        menuCanvas.GetComponentsInChildren<UnityEngine.UI.Slider>(true)
            .Where(go => go.CompareTag("MP Slider"))
    );

    expSlider.Clear();
    expSlider.AddRange(
        menuCanvas.GetComponentsInChildren<UnityEngine.UI.Slider>(true)
            .Where(go => go.CompareTag("EXP Slider"))
    );


    nameText.Clear();
    nameText.AddRange(
        menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Where(name => name.CompareTag("Character Names"))
    );

    healthText.Clear();
    healthText.AddRange(
        menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Where(health => health.CompareTag("Character Health"))
    );

    manaText.Clear();
    manaText.AddRange(
        menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Where(mana => mana.CompareTag("Character Mana"))
    );

    expText.Clear();
    expText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(exp => exp.CompareTag("Character EXP"))
    );

    levelText.Clear();
    levelText.AddRange(
        menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Where(level => level.CompareTag("Character Level"))
    );

    dexterityText.Clear();
    dexterityText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(dex => dex.CompareTag("Character Dexterity"))
    );

    physicalDEFText.Clear();
    physicalDEFText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(pdef => pdef.CompareTag("Character Physical DEF"))
    );

    magicalDEFText.Clear();
    magicalDEFText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(mdef => mdef.CompareTag("Character Magical DEF"))
    );

    strengthText.Clear();
    strengthText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(str => str.CompareTag("Character Strength"))
    );

    intelligenceText.Clear();
    intelligenceText.AddRange(
      menuCanvas.GetComponentsInChildren<TextMeshProUGUI>(true)
      .Where(intel => intel.CompareTag("Character Intelligence"))
    );

    characterImages.Clear();
    characterImages.AddRange(
        menuCanvas.GetComponentsInChildren<UnityEngine.UI.Image>(true)
            .Where(img => img.CompareTag("Character Image"))
    );

    characterSlots.Clear();
    characterSlots.AddRange(
        menuCanvas.GetComponentsInChildren<Transform>(true)
            .Select(t => t.gameObject)
            .Where(go => go.CompareTag("Character Slot"))
    );

  }

  private void ManageMenuCanvas()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      // UpdateStats();

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
}
