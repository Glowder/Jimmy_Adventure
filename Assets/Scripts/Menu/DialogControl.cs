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
  // [SerializeField] public int currentDialogIndex, dialogLength;
  [SerializeField] string[] dialog;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      if (DialogHandler.instance != null)
        DialogHandler.instance.SetDialogIndex(index: 2);

      if (NPCStats.instance != null)
        NPCStats.instance.ActivateNPCDialog();


      Debug.Log("Dialog length = " + DialogHandler.instance.GetDialogLength());
      Debug.Log("Gameobject with instance of DebugHandler: " + DialogHandler.instance.gameObject.name);
    }

    if (Input.GetKeyDown(KeyCode.C))
    {
      if (GetDialogBoxState())
      {
          DialogHandler.instance.RunDialog();
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
