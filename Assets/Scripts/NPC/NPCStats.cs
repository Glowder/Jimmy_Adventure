using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
  public static NPCStats instance;
  [SerializeField] string npcName;
  [SerializeField] Sprite npcPortrait;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
    // if (gameObject.GetComponent<DialogHandler>().enabled == true)
    gameObject.GetComponent<DialogHandler>().enabled = false;
  }

  // Update is called once per frame
  void Update()
  {

  }

  // public string GetNPCName()
  // {
  //   return npcName;
  // }
  // public Sprite GetNPCPortrait()
  // {
  //   return npcPortrait;
  // }

  public void CreateInstance()
  {
    if (instance == null)
    {
      instance = this;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      gameObject.GetComponent<DialogHandler>().enabled = true;

      DialogControl.instance.interactionIcon.SetActive(true);

      gameObject.GetComponent<DialogHandler>().CreateInstance();

      instance = this;

      DialogControl.instance.SetPorttraitImageAndName(npcPortrait, npcName);
      DialogHandler.instance.SetDialogIndex(index: 0, startDialogAtSetIndex: true, playerOrNPC: "NPC");
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
     if (DialogHandler.instance != null && DialogControl.instance != null)
     {
       gameObject.GetComponent<DialogHandler>().enabled = false;
 
       DeactivateNPCDialog();
 
       if (DialogControl.instance.interactionIcon.activeInHierarchy)
         DialogControl.instance.interactionIcon.SetActive(false);
     }
    }
  }

  public void ActivateNPCDialog()
  {
    if (DialogHandler.instance != null && DialogControl.instance != null && Player.instance != null)
    {
      DialogHandler.instance.RunDialog();
      DialogControl.instance.interactionIcon.SetActive(false);
      DialogControl.instance.dialogTextBox.SetActive(true);
      DialogControl.instance.nameTextBox.SetActive(true);
      DialogControl.instance.dialogWithNameBox.SetActive(true);
      DialogControl.instance.portraitBox.SetActive(true);
      DialogControl.instance.portraitImage.SetActive(true);
      Player.instance.DeactivateMovementAndAnimation(deactivate: true);
    }
  }

  public void DeactivateNPCDialog()
  {
    if (DialogHandler.instance != null && DialogControl.instance != null && Player.instance != null)
    {
      DialogHandler.instance.SetDialogIndex(index: 0, startDialogAtSetIndex: true, playerOrNPC: "NPC");
      DialogHandler.instance.RunDialog();
      DialogControl.instance.dialogTextBox.SetActive(false);
      DialogControl.instance.nameTextBox.SetActive(false);
      DialogControl.instance.dialogWithNameBox.SetActive(false);
      DialogControl.instance.portraitBox.SetActive(false);
      DialogControl.instance.portraitImage.SetActive(false);
      if (!DialogControl.instance.GetDialogBoxState())
        DialogControl.instance.interactionIcon.SetActive(true);
      Player.instance.DeactivateMovementAndAnimation(deactivate: false);
    }
  }

  public string GetNPCName()
  {
    return npcName;
  }

  public Sprite GetNPCPortrait()
  {
    return npcPortrait;
  }
}
