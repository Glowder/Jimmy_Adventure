using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogControl : MonoBehaviour
{
  public static DialogControl instance;
  [SerializeField] public TextMeshProUGUI dialogText, nameText;

  #region Dialog UI GameObjects
  [SerializeField]
  public GameObject
                                      dialogBox,
                                      dialogWithNameBox,
                                      interactionIcon,
                                      portraitBox,
                                      portraitImage,
                                      dialogTextBox,
                                      nameTextBox;
  #endregion
  [SerializeField] int currentDialogIndex, dialogLength;
  [SerializeField] string[] dialog;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {
    dialogLength = Player.instance.GetDialogLength()+dialog.Length;
    if (Input.GetKeyDown(KeyCode.E))
    {
      dialogText.text = dialog[0];
      if (NPCStats.instance != null)
        NPCStats.instance.ActivateNPCDialog();

    }


    if (Input.GetKeyDown(KeyCode.C))
    {
      if (currentDialogIndex >= dialogLength)
      {
        NPCStats.instance.DeactivateNPCDialog();
        currentDialogIndex = 0;
      }
      // else
      // {
      //   dialogText.text = dialog[currentDialogIndex];
      //   currentDialogIndex++;
      // }
      if (currentDialogIndex == 0 || currentDialogIndex == 2)
      {
        if (currentDialogIndex == 0)
        {
          dialogText.text = dialog[0];
          SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
          currentDialogIndex++;
        }
        else if (currentDialogIndex == 2)
        {
          dialogText.text = dialog[1];
          SetPorttraitImageAndName(NPCStats.instance.GetNPCPortrait(), NPCStats.instance.GetNPCName());
          currentDialogIndex++;
        }
      }
      else if (currentDialogIndex == 1 || currentDialogIndex == 3)
      {
        if (currentDialogIndex == 1)
        {
          dialogText.text = Player.instance.dialog[0];
          SetPorttraitImageAndName(Player.instance.GetPlayerPortrait(), Player.instance.GetPlayerName());
          currentDialogIndex++;
        }
        else if (currentDialogIndex == 3)
        {
          dialogText.text = Player.instance.dialog[1];
          SetPorttraitImageAndName(Player.instance.GetPlayerPortrait(), Player.instance.GetPlayerName());
          currentDialogIndex++;
        }
      }
    }
  }

  public void SetPorttraitImageAndName(Sprite portrait, string name)
  {
    // if (portraitImage != null)
    portraitImage.GetComponent<Image>().sprite = portrait;
    nameText.text = name;
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
}
