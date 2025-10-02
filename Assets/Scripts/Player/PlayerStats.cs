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
}
