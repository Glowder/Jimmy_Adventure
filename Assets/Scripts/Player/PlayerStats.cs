using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats instance;
  [SerializeField] Sprite playerPortrait;
  [SerializeField] string playerName;
  [SerializeField]
  int maxLevel = 50, currentLevel = 1,
  maxXP, currentXP,
  maxHP = 100, currentHP, strength = 3,
  maxMP = 80, currentMP, intelligence = 5,
  dexterity = 2,
  physicalDEF = 2, magicalDEF = 4;

  public List<int> requiredXPForEachLevel = new List<int>();

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
    SetRequieredXPForEachLevel();
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

  public string GetPlayerName()
  {
    return playerName;
  }

  public Sprite GetPlayerPortrait()
  {
    return playerPortrait;
  }

  private void SetRequieredXPForEachLevel()
  {
    // Clears the list to avoid duplicates when relading the scene.
    requiredXPForEachLevel.Clear();


    for (int i = 0; i < maxLevel; i++)
    {
      // int xp = (int)(i + Mathf.Sqrt(i) * 17);
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
}
