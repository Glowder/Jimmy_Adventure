using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
  public static DialogHandler instance;
  public string[] dialog;
  public int currentDialogIndex;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.T))
    {
    }
  }

  public void CreateInstance()
  {
    if (instance == null || instance != this)
      instance = this;
  }

  public int GetDialogLength => dialog.Length;

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
    if (GameManager.instance != null && PlayerStats.instance != null)
    {
      PlayerStats[] playerStats =
      GameManager.instance.GetSortedPlayerStats() != null ?
      GameManager.instance.GetSortedPlayerStats() : new PlayerStats[0];


      switch (playerName)
      {
        case "Jimmy":
          return (playerStats.Length > 0 && playerStats[0] != null) ? playerStats[0].PlayerPortrait : null;
        case "Ninja":
          return (playerStats.Length > 1 && playerStats[1] != null) ? playerStats[1].PlayerPortrait : null;
        case "Ranger":
          return (playerStats.Length > 2 && playerStats[2] != null) ? playerStats[2].PlayerPortrait : null;
        case "Warrior":
          return (playerStats.Length > 3 && playerStats[3] != null) ? playerStats[3].PlayerPortrait : null;
        case "Necromancer":
          return (playerStats.Length > 4 && playerStats[4] != null) ? playerStats[4].PlayerPortrait : null;
        case "Cleric":
          return (playerStats.Length > 5 && playerStats[5] != null) ? playerStats[5].PlayerPortrait : null;

        default:
          return null;
      }
    }
    return null;
  }
}
