using UnityEngine;
using Unity.Cinemachine;


public class CamController : MonoBehaviour
{
  private Player targetPlayer;
  CinemachineCamera virtualCamera;
  [SerializeField] CinemachineConfiner2D camConfiner;
  
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    targetPlayer = Player.instance;
    virtualCamera = GetComponent<CinemachineCamera>();
    if (virtualCamera != null && targetPlayer != null)
    {
      virtualCamera.Follow = targetPlayer.transform;
    }

    camConfiner = FindAnyObjectByType<CinemachineConfiner2D>();

  }

    // Update is called once per frame
    void Update()
    {
    if (!camConfiner.BoundingShape2D)
    {
      camConfiner.BoundingShape2D = Tilemaps.instance.colliderForCamBounds;
    }

    if (targetPlayer == null)
    {
      targetPlayer = Player.instance;
      virtualCamera.Follow = targetPlayer.transform;
    }
    }
}
