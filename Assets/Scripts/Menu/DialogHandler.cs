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
    // Debug.Log("DialogHandler Instance Created from DialogHandler Start");
    Debug.Log("Instance created from Start (DialogHandler) >>> Gameobject with instance of DebugHandler: " + gameObject.name);
  }

  // Update is called once per frame
  void Update()
  {

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
      Debug.Log("Current (NPC) Dialog Index: " + currentDialogIndex);
      if (DialogControl.instance != null && NPCStats.instance != null)
      {
        DialogControl.instance.SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
        Debug.Log("Current (NPC) dialog text reached!");
      }
      currentDialogIndex++;
      Debug.Log("Current (2. NPC) Dialog Index: " + currentDialogIndex);
    }

    else if (currentDialogIndex < dialog.Length && dialog[currentDialogIndex].StartsWith("#Player"))
    {
      currentDialogIndex++;
      Debug.Log("Current (Player) Dialog Index: " + currentDialogIndex);
      if (DialogControl.instance != null && PlayerStats.instance != null)
      {
        DialogControl.instance.SetPorttraitImageAndName(PlayerStats.instance.GetPlayerPortrait(), PlayerStats.instance.GetPlayerName());
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
        Debug.Log("Current (Player) dialog text reached!");
      }
      currentDialogIndex++;
      Debug.Log("Current (2. Player) Dialog Index: " + currentDialogIndex);
    }

    else
    {
      if (currentDialogIndex < dialog.Length)
      {
        DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
        currentDialogIndex++;
      }
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
    }

  }
  public void SetDialogIndex(int index, bool startDialogAtSetIndex, string playerOrNPC)
  {
    currentDialogIndex = index;

    if (startDialogAtSetIndex)
      DialogControl.instance.dialogText.text = dialog[currentDialogIndex];

    if (playerOrNPC == "NPC")
      DialogControl.instance.SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());

    else if (playerOrNPC == "Player")
      DialogControl.instance.SetPorttraitImageAndName(PlayerStats.instance.GetPlayerPortrait(), PlayerStats.instance.GetPlayerName());
    else
      DialogControl.instance.SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
  }
}
