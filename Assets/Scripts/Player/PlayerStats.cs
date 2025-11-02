using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats instance;
  private Dictionary<string, ItemManager> playerEquipment = new Dictionary<string, ItemManager>(6);
  public readonly int maxEquipmentSlots = 6;
  [SerializeField] Sprite playerPortrait;
  [SerializeField] string playerName, playerClass;
  [SerializeField]
  int groupPositionNumber, maxLevel, currentLevel = 1,
  maxXP, currentXP,
  maxHP, currentHP, strength,
  maxMP, currentMP, intelligence,
  dexterity, crit,
  physicalDEF, physicalEvasion, magicDEF, magicEvasion;

  public List<int> requiredXPForEachLevel = new List<int>();

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
    SetRequieredXPForEachLevel();
    SetCharacterEquipment();
  }

  // Update is called once per frame
  void Update()
  {
    PlayerLevelingManagement();
  }

  private void CreateInstance()
  {
    if (instance == null || instance != this)
      instance = this;
  }

  private void SetCharacterEquipment()
  {
    playerEquipment.Add("Head", null);
    playerEquipment.Add("Chest", null);
    playerEquipment.Add("Arms", null);
    playerEquipment.Add("Legs", null);
    playerEquipment.Add("Weapon", null);
    playerEquipment.Add("Shield", null);
  }

  public void EquipItemUI(string slot, ItemManager item)
  {
    if (playerEquipment.ContainsKey(slot))
    {
      // NOTE: If player has no equipment at [slot] then the item will be equipped
      if (playerEquipment[slot] == null)
      {
        playerEquipment[slot] = item;
        Debug.Log($"Equipped {item.itemName} to {slot} slot.");
      }
      // NOTE: If player is equipped at [slot] the item at [slot] will be added to inventory and removed from player
      else if (playerEquipment[slot] != null)
      {
        Inventory.instance.AddItem(playerEquipment[slot]);
        playerEquipment[slot] = item;
        Debug.Log($"Equipped {item.itemName} to {slot} slot.");
      }
    }
    else
    {
      Debug.LogWarning($"Slot {slot} does not exist in player equipment.");
    }
  }

  private void SetRequieredXPForEachLevel()
  {
    // Clears the list to avoid duplicates when relading the scene.
    requiredXPForEachLevel.Clear();


    for (int i = 0; i < maxLevel; i++)
    {
      // int xp = (int)(Mathf.Pow(i, 2) * 15);   // z.B. quadratisch
      // int xp = i * 100;                       // lineare Kurve
      int xp = (int)(Mathf.Pow(1.15f, i) * 36); // exponentiell wachsend

      requiredXPForEachLevel.Add(xp);
    }
  }

  private void PlayerLevelingManagement()
  {
    // Safety check for the list
    if (requiredXPForEachLevel == null || requiredXPForEachLevel.Count == 0)
      return;

    if (currentLevel < maxLevel)
    {
      if (Input.GetKeyDown(KeyCode.L))
        currentXP += currentLevel * 100;

      // Ensure currentLevel - 1 is within bounds
      int levelIndex = Mathf.Min(currentLevel - 1, requiredXPForEachLevel.Count - 1);
      levelIndex = Mathf.Max(0, levelIndex);

      if (currentXP >= requiredXPForEachLevel[levelIndex])
      {
        currentXP -= requiredXPForEachLevel[levelIndex];
        currentLevel++;
        // Debug.Log($"Player leveled up! New Level: {currentLevel}");

        // Safe access for next level XP display
        if (currentLevel - 1 < requiredXPForEachLevel.Count)
        {
          // Debug.Log($"Required XP for next level: {requiredXPForEachLevel[currentLevel - 1]}");
        }
        AddStatsAfterLevelUp();
      }
      if (currentLevel == maxLevel)
        currentXP = 0;
    }
  }

  public void SetDetailedStatsValues()
  {
    if (MenuManager.instance != null && instance != null && ItemMenuManager.instance != null)
    {
      MenuManager stats = MenuManager.instance;
      ItemMenuManager equipmentInventory = ItemMenuManager.instance;
      string[] equipmentSlots = new string[6] { "Head", "Chest", "Arms", "Legs", "Weapon", "Shield" };
      string set = "Set";
      string remove = "Remove";

      stats.DSNameText.text = playerName;
      stats.DSPortrait.sprite = playerPortrait;
      stats.DSHealthText.text = currentHP + " / " + maxHP;
      stats.DSHealthSlider.maxValue = maxHP;
      stats.DSHealthSlider.value = currentHP;
      stats.DSManaText.text = currentMP + " / " + maxMP;
      stats.DSManaSlider.maxValue = maxMP;
      stats.DSManaSlider.value = currentMP;
      stats.DSExpText.text = currentXP + " / " + maxXP;
      stats.DSLevelText.text = "Level: " + currentLevel;
      stats.DSStrengthText.text = "Strength: \n" + strength;
      stats.DSIntelligenceText.text = "Intelligence: \n" + intelligence;
      stats.DSCritText.text = "Critical: \n" + crit + "%";
      stats.DSDexterityText.text = "Dexterity: \n" + dexterity;
      stats.DSPhysicalDEFText.text = "Physical DEF: \n" + physicalDEF;
      stats.DSMagicDEFText.text = "Magical DEF: \n" + magicDEF;
      stats.DSPhysicalEvasion.text = "Physical Evasion: \n" + physicalEvasion + "%";
      stats.DSMagicEvasion.text = "Magical Evasion: \n" + magicEvasion + "%";

      for (int i=0; i < playerEquipment.Count; i++)
      {
        string slot = equipmentSlots[i];
        if (playerEquipment[slot] != null)
        {
          equipmentInventory.SetEquipmentUIElements(set, slot, playerEquipment[slot].itemIcon);
        }
        else
        {
          equipmentInventory.SetEquipmentUIElements(remove, slot, stats.ItemFrame);
        }
      }
    }
  }

  public void AddStatsAfterLevelUp()
  {
    maxHP += Mathf.FloorToInt(Mathf.Sqrt(currentLevel * 2.73f) * Mathf.Sqrt(currentLevel * 1.19f));

    maxMP += Mathf.FloorToInt(Mathf.Sqrt(currentLevel * 0.73f) * Mathf.Sqrt(currentLevel * 1.19f));

    strength += Mathf.FloorToInt(Mathf.Sqrt(currentLevel * 0.23f) * Mathf.Sqrt(currentLevel * 0.39f));

    intelligence += Mathf.FloorToInt(Mathf.Sqrt(currentLevel * 0.33f) * Mathf.Sqrt(currentLevel * 0.59f));

    dexterity += 2;

    physicalDEF = Mathf.FloorToInt(strength * 1.23f);

    magicDEF = Mathf.FloorToInt(intelligence * 1.35f);
  }


  #region "Getters and Setters"

  public int PlayerGroupPositionNumber => groupPositionNumber;
  public int MaxEquipmentSlots
  {
    get => maxEquipmentSlots;
  }
  public int CritChance
  {
    get => crit;
    set => crit = value;
  }
  public string PlayerName
  {
    get => playerName;
  }

  public Sprite PlayerPortrait
  {
    get => playerPortrait;
  }

  public int Health
  {
    get => currentHP;
    set => currentHP = value;
  }

  public int Mana
  {
    get => currentMP;
    set => currentMP = value;
  }

  public int Experience
  {
    get => currentXP;
    set => currentXP = value;
  }

  public int Level
  {
    get => currentLevel;
    set => currentLevel = value;
  }

  public int Dexterity
  {
    get => dexterity;
    set => dexterity = value;
  }
  public int GroupPositionNumber
  {
    get => groupPositionNumber;
  }
  public string PlayerClass
  {
    get => playerClass;
  }
  public int MaxHealth
  {
    get => maxHP;
    set => maxHP = value;
  }
  public int MaxMana
  {
    get => maxMP;
    set => maxMP = value;
  }
  public int Strength
  {
    get => strength;
    set => strength = value;
  }
  public int Intelligence
  {
    get => intelligence;
    set => intelligence = value;
  }
  public int PhysicalDEF
  {
    get => physicalDEF;
    set => physicalDEF = value;
  }
  public int MagicalDEF
  {
    get => magicDEF;
    set => magicDEF = value;
  }
  public int MaxLevel
  {
    get => maxLevel;
  }
  public int GetMaxXP()
  {
    // Safety checks
    if (currentLevel < 1)
      currentLevel = 1;

    if (requiredXPForEachLevel == null || requiredXPForEachLevel.Count == 0)
    {
      Debug.LogWarning("requiredXPForEachLevel list is empty or null!");
      return maxXP = 100; // Return a default value
    }

    // Ensure we don't go out of bounds and use 0-based indexing
    int levelIndex = Mathf.Min(currentLevel - 1, requiredXPForEachLevel.Count - 1);
    levelIndex = Mathf.Max(0, levelIndex); // Ensure index is never negative

    maxXP = requiredXPForEachLevel[levelIndex];
    return maxXP;
  }

  public ItemManager GetEquipmentAtSlot(string slot)
  {
    if (playerEquipment.ContainsKey(slot))
    {
      return playerEquipment[slot];
    }
    else
    {
      Debug.LogWarning($"Slot {slot} does not exist in player equipment.");
      return null;
    }
  }

  public int PhysicalEvasion
  {
    get => physicalEvasion;
    set => physicalEvasion = value;
  }

  public int MagicalEvasion
  {
    get => magicEvasion;
    set => magicEvasion = value;
  }
  #endregion

}
