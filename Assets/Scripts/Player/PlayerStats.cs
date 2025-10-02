using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats instance;
  [SerializeField] Sprite playerPortrait;
  [SerializeField] string playerName;



  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstance();
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void CreateInstance()
  {
    if (instance == null || instance != this)
      instance = this;

    Debug.Log("Player instance created from PlayerStats.");
  }
    
  public string GetPlayerName()
  {
    return playerName;
  }

  public Sprite GetPlayerPortrait()
  {
    return playerPortrait;
  }
}
