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

    if (dialog[currentDialogIndex].StartsWith("#NPC"))
    {
      currentDialogIndex++;
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
      DialogControl.instance.SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
      DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
      currentDialogIndex++;
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
    }

    else if (dialog[currentDialogIndex].StartsWith("#Player"))
    {
      currentDialogIndex++;
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
      DialogControl.instance.SetPorttraitImageAndName(Player.instance.GetPlayerPortrait(), Player.instance.GetPlayerName());
      DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
      currentDialogIndex++;
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
    }

    else
    {
      DialogControl.instance.dialogText.text = dialog[currentDialogIndex];
      currentDialogIndex++;
      Debug.Log("Current Dialog Index: " + currentDialogIndex);
    }

  }
  public void SetDialogIndex(int index)
  {
    currentDialogIndex = index;

  }
}
