using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats instance;
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
    // maxXP = requiredXPForEachLevel[currentLevel - 1];
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

    // Debug.Log("Player instance created from PlayerStats.");
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
    MenuManager.instance.DSNameText.text = playerName;
    MenuManager.instance.DSPortrait.sprite = playerPortrait;
    MenuManager.instance.DSHealthText.text = currentHP + " / " + maxHP;
    MenuManager.instance.DSHealthSlider.maxValue = maxHP;
    MenuManager.instance.DSHealthSlider.value = currentHP;
    MenuManager.instance.DSManaText.text = currentMP + " / " + maxMP;
    MenuManager.instance.DSManaSlider.maxValue = maxMP;
    MenuManager.instance.DSManaSlider.value = currentMP;
    MenuManager.instance.DSExpText.text = currentXP + " / " + maxXP;
    MenuManager.instance.DSLevelText.text = "Level: " + currentLevel;
    MenuManager.instance.DSStrengthText.text = "Strength: " + strength;
    MenuManager.instance.DSIntelligenceText.text = "Intelligence: " + intelligence;
    MenuManager.instance.DSCritText.text = "Critical: " + crit + "%";
    MenuManager.instance.DSDexterityText.text = "Dexterity: " + dexterity;
    MenuManager.instance.DSPhysicalDEFText.text = "Physical DEF: " + physicalDEF;
    MenuManager.instance.DSMagicDEFText.text = "Magical DEF: " + magicDEF;
    MenuManager.instance.DSPhysicalEvasion.text = "Physical Evasion: " + physicalEvasion + "%";
    MenuManager.instance.DSMagicEvasion.text = "Magical Evasion: " + magicEvasion + "%";
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


  #region Getters
  public int GetCritChance()
  {
    return crit;
  }
  public string GetPlayerName()
  {
    return playerName;
  }

  public Sprite GetPlayerPortrait()
  {
    return playerPortrait;
  }

  public int GetHealth()
  {
    return currentHP;
  }

  public int GetMana()
  {
    return currentMP;
  }

  public int GetExperience()
  {
    return currentXP;
  }

  public int GetLevel()
  {
    return currentLevel;
  }

  public int GetDexterity()
  {
    return dexterity;
  }
  public int GetGroupPositionNumber()
  {
    return groupPositionNumber;
  }
  public string GetPlayerClass()
  {
    return playerClass;
  }
  public int GetMaxHealth()
  {
    return maxHP;
  }
  public int GetMaxMana()
  {
    return maxMP;
  }
  public int GetStrength()
  {
    return strength;
  }
  public int GetIntelligence()
  {
    return intelligence;
  }
  public int GetPhysicalDEF()
  {
    return physicalDEF;
  }
  public int GetMagicalDEF()
  {
    return magicDEF;
  }
  public int GetMaxLevel()
  {
    return maxLevel;
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

  public int GetPhysicalEvasion()
  {
    return physicalEvasion;
  }

  public int GetMagicalEvasion()
  {
    return magicEvasion;
  }
  #endregion

}
