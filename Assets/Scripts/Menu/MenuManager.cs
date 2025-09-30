using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
  public static MenuManager instance;
  [SerializeField] Image image;
  [SerializeField] Animator animator;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    image.gameObject.SetActive(true);
    // image = GameObject.Find("Fading").GetComponent<Image>();
    image = GetComponentInChildren<Image>();

    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {
    if (!image)
    {
      image = GetComponentInChildren<Image>();
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
