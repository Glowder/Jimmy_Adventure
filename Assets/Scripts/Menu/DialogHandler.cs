using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
  public static DialogHandler instance;
  public string[] dialog;
  public int currentDialogIndex, testIndex;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
    testIndex = 0;
    Debug.Log("DialogHandler Instance Created from DialogHandler Start");
    Debug.Log("Instance created from Start (DialogHandler) >>> Gameobject with instance of DebugHandler: " + gameObject.name);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.T))
    {
      if(testIndex >= 4) testIndex = 0;
      if (DialogControl.instance != null)
      {
        GetSortedPlayerStats2(testIndex);
        testIndex++;
        Debug.Log($"Test Index is now: {testIndex} and GetSortedPlayerStats2 is working.");
      }
      else Debug.Log("DialogControl.instance is null");
    }
  }

  public void CreateInstance()
  {
    if (instance == null || instance != this)
      instance = this;
  }

  public int GetDialogLength()
  {
    return dialog.Length;
  }

  public void RunDialog()
  {
    if (currentDialogIndex >= dialog.Length)
    {
      NPCStats.instance.DeactivateNPCDialog();
    }

    else if (currentDialogIndex < dialog.Length && dialog[currentDialogIndex].StartsWith("#NPC"))
    {
      currentDialogIndex++;
      if (DialogControl.instance != null && NPCStats.instance != null)
      {
        DialogControl.instance.SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
      }
      currentDialogIndex++;
    }

    else if (currentDialogIndex < dialog.Length && dialog[currentDialogIndex].StartsWith("#Player"))
    {
      currentDialogIndex++;
      if (DialogControl.instance != null && PlayerStats.instance != null)
      {
        DialogControl.instance.SetPorttraitImageAndName(portrait: GetPlayerPortraitForDialog("Jimmy"), name: "Jimmy");
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
      }
      currentDialogIndex++;
    }

    else
    {
      if (currentDialogIndex < dialog.Length)
      {
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
        currentDialogIndex++;
      }
    }

  }
  public void SetDialogIndex(int index, bool startDialogAtSetIndex, string playerOrNPC)
  {
    currentDialogIndex = index;

    if (startDialogAtSetIndex)
      DialogControl.instance.dialogText.text = dialog[currentDialogIndex];

    if (playerOrNPC == "NPC")
      DialogControl.instance.SetPorttraitImageAndName(portrait: NPCStats.instance.GetNPCPortrait(), name: NPCStats.instance.GetNPCName());

    else if (playerOrNPC == "Player")
      DialogControl.instance.SetPorttraitImageAndName(portrait: GetPlayerPortraitForDialog("Jimmy"), name: "Jimmy");
    else
      DialogControl.instance.SetPorttraitImageAndName(portrait: NPCStats.instance.GetNPCPortrait(), name: NPCStats.instance.GetNPCName());
  }

  private Sprite GetPlayerPortraitForDialog(string playerName)
  {
    PlayerStats[] playerStats = GetSortedPlayerStats() != null ? GetSortedPlayerStats() : new PlayerStats[0];
    switch (playerName)
    {
      case "Jimmy":
        return (playerStats.Length > 0 && playerStats[0] != null) ? playerStats[0].GetPlayerPortrait() : null;
      case "Ninja":
        return (playerStats.Length > 1 && playerStats[1] != null) ? playerStats[1].GetPlayerPortrait() : null;
      case "Ranger":
        return (playerStats.Length > 2 && playerStats[2] != null) ? playerStats[2].GetPlayerPortrait() : null;
      case "Warrior":
        return (playerStats.Length > 3 && playerStats[3] != null) ? playerStats[3].GetPlayerPortrait() : null;
      case "Mage":
        return (playerStats.Length > 4 && playerStats[4] != null) ? playerStats[4].GetPlayerPortrait() : null;
      case "Cleric":
        return (playerStats.Length > 5 && playerStats[5] != null) ? playerStats[5].GetPlayerPortrait() : null;

      default:
        return null;
    }
  }

  private PlayerStats[] GetSortedPlayerStats()
  {
    PlayerStats[] playerStats = GameManager.instance != null ? GameManager.instance.GetPlayerStats() : null;
    Debug.Log("PlayerStats array length: " + (playerStats != null ? playerStats.Length.ToString() : "null"));

    if (playerStats == null || playerStats.Length == 0)
    {
      Debug.LogError("No PlayerStats found in GameManager or array is empty.");
      return new PlayerStats[0];
    }

    PlayerStats[] sortedPlayerStats = new PlayerStats[playerStats.Length];

    // Properly sort players based on their groupPositionNumber
    for (int i = 0; i < playerStats.Length; i++)
    {
      if (playerStats[i] != null)
      {
        int targetPosition = playerStats[i].GetGroupPositionNumber();
        // Make sure the target position is within bounds
        if (targetPosition >= 0 && targetPosition < sortedPlayerStats.Length)
        {
          sortedPlayerStats[targetPosition] = playerStats[i];
          Debug.Log($"Placed {playerStats[i].GetPlayerName()} (from array index {i}) at position {targetPosition}");
        }
        else
        {
          Debug.LogWarning($"Player {playerStats[i].GetPlayerName()} has invalid groupPositionNumber: {targetPosition}");
        }
      }
    }

    return sortedPlayerStats;
  }
  private PlayerStats[] GetSortedPlayerStats2(int testIndex)
  {
    PlayerStats[] playerStats = GameManager.instance != null ? GameManager.instance.GetPlayerStats() : null;
    Debug.Log("PlayerStats array length: " + (playerStats != null ? playerStats.Length.ToString() : "null"));

    if (playerStats == null || playerStats.Length == 0)
    {
      Debug.LogError("No PlayerStats found in GameManager or array is empty.");
      return new PlayerStats[0];
    }

    PlayerStats[] sortedPlayerStats = new PlayerStats[playerStats.Length];

    // Properly sort players based on their groupPositionNumber
    for (int i = 0; i < playerStats.Length; i++)
    {
      if (playerStats[i] != null)
      {
        int targetPosition = playerStats[i].GetGroupPositionNumber();
        // Make sure the target position is within bounds
        if (targetPosition >= 0 && targetPosition < sortedPlayerStats.Length)
        {
          sortedPlayerStats[targetPosition] = playerStats[i];
          Debug.Log($"Placed {playerStats[i].GetPlayerName()} at position {targetPosition}");
        }
        else
        {
          Debug.LogWarning($"Player {playerStats[i].GetPlayerName()} has invalid groupPositionNumber: {targetPosition}");
        }
      }
    }

    if (DialogControl.instance != null)
    {
      DialogControl.instance.SetTestImageAndName(sortedPlayerStats[testIndex].GetPlayerPortrait(), sortedPlayerStats[testIndex].GetPlayerName());
    }
    else Debug.Log("DialogControl.instance is null");

    Debug.Log($"Player at index {testIndex} is: {sortedPlayerStats[testIndex].GetPlayerName()}");
    Debug.Log($"Player at index {testIndex} is: {sortedPlayerStats[testIndex].GetPlayerPortrait()}");

    return sortedPlayerStats;
  }

}
