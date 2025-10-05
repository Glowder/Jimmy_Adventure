using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

  public static Player instance;
  public string lastUsedPortalName;
  public bool horizontalMove, verticalMove;
  [SerializeField] private Rigidbody2D playerRigidBody;
  [SerializeField] private Animator playerAnimator;
  [SerializeField] float moveSpeed = 2f;

  private Vector3 bottomLeft, topRight;
  public bool newMapLoaded = false;
  public bool loadingNewMap = false;

  void Start()
  {
    SetInstanceAndKeepPlayer();
  }

  void Update()
  {

    if (newMapLoaded)
    {
      SetMapEdgeLimits(bottomLeft, topRight);
      newMapLoaded = false;
    }
    PlayerMovement();

  }

  // Handles the player movement and animations.
  private void PlayerMovement()
  {
    // Gets the player input from the Input Manager.
    float horizontalMovement = Input.GetAxis("Horizontal");
    float verticalMovement = Input.GetAxis("Vertical");

    // Sets the player animator parameters to control the animations.

    playerAnimator.SetFloat("walk_x", playerRigidBody.linearVelocity.x);
    playerAnimator.SetFloat("walk_y", playerRigidBody.linearVelocity.y);


    // Sets the last direction the player moved in.
    if (horizontalMovement != 0 || verticalMovement != 0)
    {
      playerAnimator.SetFloat("lastPos_x", horizontalMovement);
      playerAnimator.SetFloat("lastPos_y", verticalMovement);
    }

    // Player runs when Left Shift is pressed.
    moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 5f : 2f;

    // Tries to lock the player movement to one axis at a time.
    horizontalMove = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ? true : false;
    verticalMove = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) && !horizontalMove ? true : false;

    // Player moves at one axis at a time, depending on whitch axis is locked.

    if (!verticalMove)
    {
      playerRigidBody.linearVelocity = new Vector2(horizontalMovement, 0f) * moveSpeed;
    }

    if (!horizontalMove)
    {
      playerRigidBody.linearVelocity = new Vector2(0f, verticalMovement) * moveSpeed;
    }

    // Clamp player position to map edges so the player can´t walt outside the map.
    transform.position = new Vector3
    (
      Mathf.Clamp(transform.position.x, bottomLeft.x, topRight.x),
      Mathf.Clamp(transform.position.y, bottomLeft.y, topRight.y),
      Mathf.Clamp(transform.position.z, bottomLeft.z, topRight.z)
    );

  }

  public void SetMapEdgeLimits(Vector3 bottomLeft, Vector3 topRight)
  {
    this.bottomLeft = bottomLeft;
    this.topRight = topRight;
  }

  private void SetInstanceAndKeepPlayer()
  {
    if (instance != null && instance != this)
    {
      Destroy(gameObject);
    }
    else { instance = this; }
    DontDestroyOnLoad(gameObject);

  }




  // Checks if the dialog box is active and disables player movement and animation if ture.
  // If false the player movement and animation is enabled again.
  public void DeactivateMovementAndAnimation(bool deactivate)
  {
    if (deactivate)
    {
      playerAnimator.enabled = false;
      playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
      Debug.Log("Player movement deactivated from Player script");
    }
    else
    {
      playerAnimator.enabled = true;
      playerRigidBody.constraints = RigidbodyConstraints2D.None;
      playerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
      Debug.Log("Player movement activated from Player script");
    }
  }
}

