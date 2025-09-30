using Microsoft.Unity.VisualStudio.Editor;
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
      DialogControl.instance.interactionIcon.SetActive(true);
      instance = null;
      instance = this;
    }
  }
  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      DialogControl.instance.interactionIcon.SetActive(false);
      DeactivateNPCDialog();
    }
  }

  public void ActivateNPCDialog()
  {
    DialogControl.instance.interactionIcon.SetActive(false);
    DialogControl.instance.dialogTextBox.SetActive(true);
    DialogControl.instance.nameTextBox.SetActive(true);
    DialogControl.instance.dialogWithNameBox.SetActive(true);
    DialogControl.instance.portraitBox.SetActive(true);
    DialogControl.instance.portraitImage.SetActive(true);
    // DialogControl.instance.nameText.text = npcName;
    // DialogControl.instance.SetPorttraitImage(npcPortrait);
  }

  public void DeactivateNPCDialog()
  {
    DialogControl.instance.dialogTextBox.SetActive(false);
    DialogControl.instance.nameTextBox.SetActive(false);
    DialogControl.instance.dialogWithNameBox.SetActive(false);
    DialogControl.instance.portraitBox.SetActive(false);
    DialogControl.instance.portraitImage.SetActive(false);
    // DialogControl.instance.nameText.text = "";
    // DialogControl.instance.SetPorttraitImage(null);
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
