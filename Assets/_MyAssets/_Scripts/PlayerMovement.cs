using UnityEngine;
using System.Collections;

// The GameObject requires a Rigidbody component
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Tooltip("Array of targets, where the player can move to, don't mess with this.")]
    /// <summary>
    /// Array of targets, where the player can move to.
    /// </summary>
    [SerializeField]
    private Transform[] targets;

    [Tooltip("Speed at which the player will move between planes.")]
    /// <summary>
    /// Speed of the movement between planes.
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// What target to move to next.
    /// </summary>
    private int targetIndex;

    /// <summary>
    /// Has the character arrived at the target position?
    /// </summary>
    private bool arrivedAtTarget = false;

    /// <summary>
    /// Holds the current position and the next position, used to interpolate movement between positions.
    /// </summary>
    private Vector3 currentPos, nextPos;

    [Tooltip("Force at which the player will jump, affects height.")]
    /// <summary>
    /// Force of the character's jump.
    /// </summary>
    public float jumpForce = 1.0f;

    /// <summary>
    /// Is the player on the ground, can he jump?
    /// </summary>
    private bool isGrounded = false;

    /// <summary>
    /// Player's rigidbody.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Is the game over?
    /// </summary>
    private bool gameOver = false;

    #endregion Variables

    #region Monobehaviour

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Update () {
        if (!gameOver)
        {
            CheckInput();       // For testing
            MoveToPosition();
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))  // Check if player is grounded before player can jump again.
            isGrounded = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))             // Check if player has arrived at target.
            arrivedAtTarget = true;

        if (other.CompareTag("Hand"))               // If the hand has grabbed the player.
        {
            gameOver = true;
            other.gameObject.GetComponentInParent<HandMovement>().SetGrabbed();
        }
    }

    #endregion Monobehaviour

    #region Public Methods

    /// <summary>
    /// Moves the character up a plane.
    /// </summary>
    public void MoveUp()
    {
        if (isGrounded && arrivedAtTarget)
        {
            if (targetIndex < (targets.Length - 1))
            {
                targetIndex++;
                arrivedAtTarget = false;
            }
            //Debug.Log("Move Up");
        }
    }

    /// <summary>
    /// Moves the character down a plane.
    /// </summary>
    public void MoveDown()
    {
        if (isGrounded && arrivedAtTarget)
        {
            if (targetIndex > 0)
            {
                targetIndex--;
                arrivedAtTarget = false;
            }
            //Debug.Log("Move Down");
        }
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    /// <summary>
    /// Attach character to the Transform passed into the method, preferably the hand's Transform.
    /// </summary>
    /// <param name="p">The transform of whatever you want the character's parent  to be.</param>
    public void AttachToHand(Transform p)
    {
        if (gameOver)
        {
            transform.parent = p;
            rb.isKinematic = true;
        }
    }

    /// <summary>
    /// Get the index of the plane the player is currently on.
    /// </summary>
    /// <returns>Returns an int that is the index of the plane the player is currently on.</returns>
    public int GetPlayerPlane()
    {
        return targetIndex;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// For testing.  This changes the target index, which in turn changes where the player's next destination is.
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveDown();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveUp();
        if (Input.GetKeyDown(KeyCode.A))
            targetIndex = 2;
        if (Input.GetKeyDown(KeyCode.S))
            targetIndex = 1;
        if (Input.GetKeyDown(KeyCode.D))
            targetIndex = 0;
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    /// <summary>
    /// Moves player from one point to another.
    /// </summary>
    private void MoveToPosition()
    {
        if (isGrounded)
        {
            currentPos = this.transform.position;       // Set current position to the player's current postion
            nextPos = targets[targetIndex].position;    // Sets the next position to one of the targets, indicated by the target index.

            // Interpolates the player's position between current position and destination.
            transform.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * speed);
        }
    }

    #endregion Private Methods
}
