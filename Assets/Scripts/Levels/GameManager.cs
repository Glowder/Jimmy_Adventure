using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  [SerializeField] PlayerStats[] playerStats;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreateInstanceAndKeepAlive();
    playerStats = FindObjectsByType<PlayerStats>(FindObjectsSortMode.None);
  }

  // Update is called once per frame
  void Update()
    {
        
    }

  private void CreateInstanceAndKeepAlive()
  {
    if (instance != null && instance != this)
    {
      Destroy(this.gameObject);
    }
    else
      instance = this;

    DontDestroyOnLoad(this.gameObject);
  }

  public PlayerStats[] GetSortedPlayerStats()
  {
    // NOTE: Return sorted PlayerStats array
    PlayerStats[] tempStats = new PlayerStats[playerStats.Length];
    for (int i = 0; i < playerStats.Length; i++)
    {
      tempStats[playerStats[i].PlayerGroupPositionNumber] = playerStats[i];
    }
    return tempStats;
  }
  
  public PlayerStats ManualPlayerReturnForButtons(int index)
  {
    return GetSortedPlayerStats()[index];
  }
}
