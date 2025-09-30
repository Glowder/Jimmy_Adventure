using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// PortalEnter is the trigger area the player has to enter to load a new region via a portal
public class PortalEnter : MonoBehaviour
{

  // The PortalExit object that defines where the player will spawn in the new region
  [SerializeField] PortalExit portalToLeaveRegion;
  public float waitingTime = 2f;

  // The name of the scene to load when the player enters the portal
  [SerializeField] string sceneToLoad;

  // The name of the portal, used to match with the PortalExit in the new region
  public string portalName;
  [SerializeField] GameObject playerObject;

  void Start()
  {
    // Set the value for PortalExit regionStartingPoint to the name of the scene to load
    portalToLeaveRegion.regionStartingPoint = sceneToLoad;
  }

  void Update()
  {

  }

  // When the player enters the portal trigger area,
  // load the new scene and set the last used portal name
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      Player.instance.lastUsedPortalName = portalName;

      Player.instance.loadingNewMap = true;

      if (MenuManager.instance != null)
        MenuManager.instance.FadeImage();

      StartCoroutine(LoadSceneCoroutine(waitingTime));
      
    }
  }

  IEnumerator LoadSceneCoroutine(float time)
  {
    yield return new WaitForSeconds(time);
    SceneManager.LoadScene(sceneToLoad);
  }
}
