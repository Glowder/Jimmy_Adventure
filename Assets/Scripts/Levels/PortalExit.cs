using UnityEngine;

// PortalExit is the position the player will spawn at when entering a new region via a portal
public class PortalExit : MonoBehaviour
{
  public string regionStartingPoint;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    regionStartingPoint = (regionStartingPoint == "" || regionStartingPoint == null) ? "Start" : regionStartingPoint;

    if (Player.instance != null && regionStartingPoint == Player.instance.lastUsedPortalName)
    {
      Player.instance.transform.position = transform.position;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
