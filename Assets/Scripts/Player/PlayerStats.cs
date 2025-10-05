using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats instance;
  [SerializeField] Sprite playerPortrait;
  [SerializeField] string playerName, playerClass;
  [SerializeField]
  int groupPositionNumber, maxLevel = 50, currentLevel = 5,
  maxXP, currentXP = 25,
  maxHP = 100, currentHP = 30, strength = 3,
  maxMP = 80, currentMP = 20, intelligence = 5,
  dexterity = 2,
  physicalDEF = 2, magicalDEF = 4;

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

    Debug.Log("Player instance created from PlayerStats.");
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
    if (currentLevel < maxLevel)
    {
      if (Input.GetKeyDown(KeyCode.L))
        currentXP += currentLevel * 100;

      if (currentXP >= requiredXPForEachLevel[currentLevel - 1])
      {
        currentXP -= requiredXPForEachLevel[currentLevel - 1];
        currentLevel++;
        Debug.Log($"Player leveled up! New Level: {currentLevel}");
        Debug.Log($"Reqired XP for next level: {requiredXPForEachLevel[currentLevel - 1]}");
        AddStatsAfterLevelUp();
      }
      if (currentLevel == maxLevel)
        currentXP = 0;

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

    magicalDEF = Mathf.FloorToInt(intelligence * 1.35f);
  }

  #region Getters
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
    return magicalDEF;
  }
  public int GetMaxLevel()
  {
    return maxLevel;
  }
  public int GetMaxXP()
  {
    maxXP = requiredXPForEachLevel[currentLevel - 1];
    return maxXP;
  }
  #endregion

}
