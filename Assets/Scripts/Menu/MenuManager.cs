using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
  public static MenuManager instance;
  [SerializeField] Image image;
  [SerializeField] Animator animator;
  [SerializeField] GameObject menuCanvas;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();

    image.gameObject.SetActive(true);
    // image = GameObject.Find("Fading").GetComponent<Image>();
    image = GetComponentInChildren<Image>();

  }

  // Update is called once per frame
  void Update()
  {
    if (!image)
    {
      image = GetComponentInChildren<Image>();
    }

    if (Input.GetKeyDown(KeyCode.P))
    {
      if (!menuCanvas.activeInHierarchy)
        menuCanvas.SetActive(true);
      else
        menuCanvas.SetActive(false);
      
    }
  }

  public void FadeImage()
  {
    image.GetComponent<Animator>().SetTrigger("Start Fade");
  }

  private void CreateInstance()
  {
    if (instance != null && instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      instance = this;
    }
  }
}
