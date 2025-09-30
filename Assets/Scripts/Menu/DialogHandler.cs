using UnityEngine;

public class DialogHandler : MonoBehaviour
{
  public static DialogHandler instance;
  public string[] dialog;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {

  }
    
  public void CreateInstance()
  {
      instance = this;
  }
}
