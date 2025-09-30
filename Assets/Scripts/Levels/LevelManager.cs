using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
  public static LevelManager instance;
  [SerializeField] Tilemap currentMap;
  private Vector3 bottomLeftEdge;
  private Vector3 topRightEdge;

  void Start()
  {
    currentMap = Tilemaps.instance.currentMap;

    bottomLeftEdge = currentMap.CellToWorld(currentMap.cellBounds.min) + new Vector3(0.6f, 0.6f, 0f);
    topRightEdge = currentMap.CellToWorld(currentMap.cellBounds.max) - new Vector3(0.6f, 0.6f, 0f);
    if (Player.instance)
    {
      Player.instance.SetMapEdgeLimits(bottomLeftEdge, topRightEdge);
      Player.instance.newMapLoaded = true;
    }
  }

  void Update()
  {
    if (!currentMap)
    {
      TilemapManagament();
    }
  }

  private void TilemapManagament()
  {
    currentMap = Tilemaps.instance.currentMap;

    if (currentMap)
    {
      bottomLeftEdge = currentMap.localBounds.min + new Vector3(0.5f, 0.5f, 0f);
      topRightEdge = currentMap.localBounds.max - new Vector3(0.5f, 0.5f, 0f);
    }
    if (Player.instance)
    {
      Player.instance.SetMapEdgeLimits(bottomLeftEdge, topRightEdge);
      Player.instance.newMapLoaded = true;
    }
  }
}
