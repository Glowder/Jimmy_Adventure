using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemaps : MonoBehaviour
{
  public static Tilemaps instance;
  public Collider2D colliderForCamBounds;
  public Tilemap currentMap;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (instance != null && instance != this)
    {
      Destroy(gameObject);
    }

    else { instance = this; }

    currentMap = GetComponent<Tilemap>();
    colliderForCamBounds = GetComponent<PolygonCollider2D>();
  }

    // Update is called once per frame
    void Update()
    {
        
    }
}
