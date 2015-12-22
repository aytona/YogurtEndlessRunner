using UnityEngine;
using System.Collections;

public class HandMovement : MonoBehaviour
{

    #region Variables

    [Tooltip("Array of targets, where the hand can move to, don't mess with this.")]
    /// <summary>
    /// Array of targets, where the player can move to.
    /// </summary>
    [SerializeField]
    private Transform[] targets;

    [Tooltip("Speed at which the hand will move between planes and when grabbing.")]
    /// <summary>
    /// Speed of the movement between planes.
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// What target to move to next.
    /// </summary>
    public int targetIndex;

    /// <summary>
    /// Holds the current position and the next position, used to interpolate movement between positions.
    /// </summary>
    private Vector3 currentPos, nextPos;

    [Tooltip("Tweak the X position of the hand's grab.")]
    /// <summary>
    /// Tweaks the X position of the hand when grabbing.
    /// </summary>
    public float xTweak = 0.55f;

    [Tooltip("Tweak the Y position of the hand's grab.")]
    /// <summary>
    /// Tweaks the Y position of the hand when grabbing.
    /// </summary>
    public float yTweak = 1.0f;

    /// <summary>
    /// The x position of the hand when not attacking.
    /// </summary>
    private float neutralXPos;

    /// <summary>
    /// The y position of the hand when not attacking.
    /// </summary>
    private float neutralYPos;

    /// <summary>
    /// Can the hand move around and search for the character.
    /// </summary>
    public bool canMove = false;

    /// <summary>
    /// Can the hand move into grabbing position.
    /// </summary>
    public bool canMoveToGrab = false;

    /// <summary>
    /// Can the hand attempt to grab the character.
    /// </summary>
    public bool canGrab = false;

    [Tooltip("The Z position that the hand will hide at when not in use.")]
    /// <summary>
    /// Z position that the hand will hide at when not in use.
    /// </summary>
    public float hidePositionZ;

    /// <summary>
    /// Hide the hand from the player's view?
    /// </summary>
    public bool hideHand = false;

    /// <summary>
    /// Has the character been grabbed?
    /// </summary>
    private bool grabbedCharacter = false;

    #endregion Variables

    #region Monobehaviour

    void Start () {
        neutralXPos = transform.position.x;     // Save the x position of the hand for later use.
        neutralYPos = transform.position.y;     // Save the y position of the hand for later use.
	}

	void Update () {
        // These are called on the Update, the booleans in the methods control if they are called.
        HideHand();
        MoveToPlane();
        MoveToGrabPosition();
        AttemptGrab();
	}

    #endregion Monobehaviour

    #region Public Methods

    /// <summary>
    /// Tell the hand if it can move of not.
    /// </summary>
    /// <param name="b">True =  the hand will move; False = the hand will stop moving.</param>
    public void SetMove(bool b)
    {
        canMove = b;
    }

    /// <summary>
    /// Tell the hand if it can get ready to grab.
    /// </summary>
    /// <param name="b">True =  the hand will get into position; False = the hand will go back to its regular position.</param>
    public void SetGrabPosition(bool b)
    {
        canMoveToGrab = b;
    }

    /// <summary>
    /// Tell the hand if it can attempt a grab.
    /// </summary>
    /// <param name="b">True =  the hand attempt a grab; False = the hand will go back to its regular position.</param>
    public void SetGrab(bool b)
    {
        canGrab = b;
    }

    /// <summary>
    /// Set the next destination for the hand.
    /// </summary>
    /// <param name="n">Index of the plane that the hand will go to, 0, 1, or 2.</param>
    public void SetNextPosition(int n)
    {
        //Debug.Log("Setting Next Position to : " + n);
        targetIndex = n;
        if (targetIndex >= targets.Length)
            targetIndex = 2;
        if (targetIndex < 0)
            targetIndex = 0;
    }

    /// <summary>
    /// Hide the hand away from play.
    /// </summary>
    /// <param name="b">True =  hide hand form view; False = show hand in play.</param>
    public void SetHide(bool b)
    {
        hideHand = b;
    }

    /// <summary>
    /// Hand has grabbed the player.
    /// </summary>
    public void SetGrabbed()
    {
        grabbedCharacter = true;
    }

    /// <summary>
    /// Has the hand grabbed the character?
    /// </summary>
    /// <returns>True = Character is caught; False = Character is not caught.</returns>
    public bool HasGrabbed()
    {
        return grabbedCharacter;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Moves hand from one plane to another.  Manipulates the Z position of the hand.
    /// </summary>
    private void MoveToPlane()
    {
        if (canMove)    // If hand is set to move...
        {
            if (targetIndex > -1)
            {
                currentPos = this.transform.position;           // Set current position to the hand's current postion
                nextPos = currentPos;
                nextPos.z = targets[targetIndex].position.z;    // Sets the next position to one of the targets, indicated by the target index.

                // Interpolates the hand's position between current position and destination.
                LerpPositions(currentPos, nextPos);
            }
        }
    }

    /// <summary>
    /// Moves hand from one plane to another.  Manipulates the Z position of the hand.
    /// </summary>
    private void MoveToPlane(float zHide)
    {
        if (canMove)    // If hand is set to move...
        {
            currentPos = this.transform.position;           // Set current position to the hand's current postion
            nextPos = currentPos;
            nextPos.z = zHide;    // Sets the next position to one of the targets, indicated by the target index.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
    }

    /// <summary>
    /// Moves hand into the grabbing position.  Manipulates the X position of the hand.
    /// </summary>
    private void MoveToGrabPosition()
    {
        if (canMoveToGrab)  // If hand is set to attempt a grab...
        {
            currentPos = this.transform.position;           // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.x = targets[targetIndex].position.x + xTweak;    // Sets the next position to be above one of the targets.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
        else
        {
            currentPos = this.transform.position;           // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.x = neutralXPos;                        // Set the neutral x position of the hand back to default.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
    }

    /// <summary>
    /// Hand attemps a grab.  Manipulates the Y position of the hand.
    /// </summary>
    private void AttemptGrab()
    {
        if (canGrab)  // If hand is set to grab...
        {
            currentPos = this.transform.position;               // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.y = targets[targetIndex].position.y + yTweak;    // Sets the next position to where the plaer migth be.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
        else
        {
            currentPos = this.transform.position;               // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.y = neutralYPos;                            // Set the neutral y position of the hand back to default.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
    }

    /// <summary>
    /// Hide the hand away from play.
    /// </summary>
    private void HideHand()
    {
        if (hideHand)       // If the hand needs to be hidden...
        {
            canGrab = false;            // Can't grab
            canMoveToGrab = false;      // 
            targetIndex = -1;           // Target gets out of range
            MoveToPlane(hidePositionZ); // Move to hiding position
        }
    }

    /// <summary>
    /// Lerps from one VEctor to another.
    /// </summary>
    /// <param name="pos1">Initial Position</param>
    /// <param name="pos2">Target Position</param>
    private void LerpPositions(Vector3 pos1, Vector3 pos2)
    {
        transform.position = Vector3.Lerp(pos1, pos2, Time.deltaTime * speed);
    }

    #endregion Private Methods

}
