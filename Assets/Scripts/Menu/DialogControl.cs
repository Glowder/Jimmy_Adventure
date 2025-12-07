using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogControl : MonoBehaviour
{
  public static DialogControl instance;
  [SerializeField] GameObject testPortrait;
  [SerializeField] public TextMeshProUGUI testText, dialogText, nameText;
  [SerializeField]public GameObject dialogBox, dialogWithNameBox, interactionIcon, portraitBox, portraitImage, dialogTextBox, nameTextBox;

  [SerializeField] string[] dialog;

  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      if (interactionIcon.activeInHierarchy && !GetDialogBoxState() && !MenuManager.instance.GetMenuCanvasState())
      {
        if (NPCStats.instance != null)
          NPCStats.instance.ActivateNPCDialog();
      }
    }

    if (Input.GetKeyDown(KeyCode.C))
    {
      if (GetDialogBoxState() && DialogHandler.instance != null)
      {
        DialogHandler.instance.RunDialog();
      }
    }
  }

  public void SetPorttraitImageAndName(Sprite portrait, string name)
  {
    portraitImage.GetComponent<Image>().sprite = portrait;
    nameText.text = name;
  }
  public void SetTestImageAndName(Sprite portrait, string name)
  {
    testPortrait.GetComponent<Image>().sprite = portrait;
    testText.text = name;
  }

  public void CreateInstance()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }
  public bool GetDialogBoxState()
  {
    if (dialogBox.activeInHierarchy || dialogWithNameBox.activeInHierarchy)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}
