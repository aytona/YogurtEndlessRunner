﻿using UnityEngine;
using System.Collections;

public class HandMovement : MonoBehaviour
{

    #region Variables

    [Tooltip("Array of targets, where the hand can move to, don't mess with this.")]
    /// <summary>
    /// Array of targets, where the player can move to.
    /// </summary>
    [SerializeField]
    private Transform[] targets = null;

    [Tooltip("Speed at which the hand will move between planes and when grabbing.")]
    /// <summary>
    /// Speed of the movement between planes.
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// What target to move to next.
    /// </summary>
    private int targetIndex;

    /// <summary>
    /// Holds the current position and the next position, used to interpolate movement between positions.
    /// </summary>
    private Vector3 currentPos, nextPos;

    /// <summary>
    /// Can the hand move to hide or to grab, false if the hand is following the player.
    /// </summary>
    private bool freeMove = false;

    private bool gameOver = false;

    public Transform yogurtGrabPos;

    private Animator _animator;

    #endregion Variables

    #region Monobehaviour

    void Start () {
        targetIndex = -1;
        _animator = GetComponentInChildren<Animator>();
	}

	void Update () {
        if (!gameOver)
        {
            MoveToPosition();
            MoveToPlane();
        }
	}

    #endregion Monobehaviour

    #region Public Methods

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
    /// Set the free move to true or false.
    /// </summary>
    /// <param name="b">True is for when you want the hand to move to a specific location. False is for when you want the hand to follow the plane the player is in.</param>
    public void SetFreeMove(bool b)
    {
        freeMove = b;
    }

    /// <summary>
    /// Get the index of the lane the hand is in.
    /// </summary>
    /// <returns>Returns an int that represents the lane the hand is currently in. -1 means the hand is hiding.</returns>
    public int GetHandLaneIndex()
    {
        return targetIndex;
    }

    /// <summary>
    /// Set the position of the next position, this position is used in free move mode.
    /// </summary>
    /// <param name="t">The transform of the next position.</param>
    public void SetNextPosition(Transform t)
    {
        nextPos = t.position;
    }

    public void SetGrabAnim()
    {
        _animator.SetTrigger("Grab");
    }

    public Transform GetGrabPosition()
    {
        return yogurtGrabPos;
    }

    public void SetGameOver(bool b)
    {
        gameOver = b;
    }

    public void EndGameHide(Transform hidePosition)
    {
        freeMove = true;
        speed /= 2;
        nextPos = hidePosition.position;
        StartCoroutine(EndMoveLoop());
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Moves hand to the next position when in free move mode.
    /// </summary>
    private void MoveToPosition()
    {
        if (freeMove)
        {
            currentPos = this.transform.position;           // Set current position to the hand's current postion

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
    }

    /// <summary>
    /// Moves hand from one plane to another when not in free move mode.
    /// </summary>
    private void MoveToPlane()
    {
        if (!freeMove)
        {
            if (targetIndex > -1)
            {
                currentPos = this.transform.position;           // Set current position to the hand's current postion

                nextPos.z = targets[targetIndex].position.z;    // Sets the next position to one of the targets, indicated by the target index.
                nextPos.y = targets[targetIndex].position.y;
                nextPos.y += 1;

                // Interpolates the hand's position between current position and destination.
                LerpPositions(currentPos, nextPos);
            }
        }
    }

    /// <summary>
    /// Lerps from one Vector to another.
    /// </summary>
    /// <param name="pos1">Initial Position</param>
    /// <param name="pos2">Target Position</param>
    private void LerpPositions(Vector3 pos1, Vector3 pos2)
    {
        transform.position = Vector3.Lerp(pos1, pos2, Time.deltaTime * speed);
    }

    private IEnumerator EndMoveLoop()
    {
        MoveToPosition();
        yield return new WaitForEndOfFrame();
        StartCoroutine(EndMoveLoop());
    }

    #endregion Private Methods

    #region Old Hand Movement Variables and Methods
    /*

    [Tooltip("Tweak the X position of the hand's grab.")]
    /// <summary>
    /// Tweaks the X position of the hand when grabbing.
    /// </summary>
    public float xTweak = 0.55f;

    [Tooltip("Tweak the Z position of the hand's grab.")]
    /// <summary>
    /// Tweaks the Z position of the hand when grabbing.
    /// </summary>
    public float zTweak = 1.0f;

    /// <summary>
    /// The x position of the hand when not attacking.
    /// </summary>
    private float neutralXPos;

    /// <summary>
    /// The z position of the hand when not attacking.
    /// </summary>
    private float neutralZPos;

    /// <summary>
    /// Can the hand move around and search for the character.
    /// </summary>
    private bool canMove = false;

    /// <summary>
    /// Can the hand move into grabbing position.
    /// </summary>
    private bool canMoveToGrab = false;

    /// <summary>
    /// Can the hand attempt to grab the character.
    /// </summary>
    private bool canGrab = false;

    [Tooltip("The X position that the hand will hide at when not in use.")]
    /// <summary>
    /// X position that the hand will hide at when not in use.
    /// </summary>
    public float hidePositionX;

    /// <summary>
    /// Hide the hand from the player's view?
    /// </summary>
    private bool hideHand = false;

    /// <summary>
    /// Has the character been grabbed?
    /// </summary>
    private bool grabbedCharacter = false;

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

    /// <summary>
    /// Moves hand from one plane to another.  Manipulates the X and Y position of the hand.
    /// </summary>
    private void MoveToPlane(float xHide)
    {
        if (canMove)    // If hand is set to move...
        {
            currentPos = this.transform.position;           // Set current position to the hand's current postion
            nextPos = currentPos;
            nextPos.x = xHide;    // Sets the next position to one of the targets, indicated by the target index.

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
    /// Hand attemps a grab.  Manipulates the Z position of the hand.
    /// </summary>
    private void AttemptGrab()
    {
        if (canGrab)  // If hand is set to grab...
        {
            currentPos = this.transform.position;               // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.z = targets[targetIndex].position.z + zTweak;    // Sets the next position to where the plaer migth be.

            // Interpolates the hand's position between current position and destination.
            LerpPositions(currentPos, nextPos);
        }
        else
        {
            currentPos = this.transform.position;               // Set current position to the hand's current postion.
            nextPos = currentPos;
            nextPos.z = neutralZPos;                            // Set the neutral y position of the hand back to default.

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
            MoveToPlane(hidePositionX); // Move to hiding position
        }
    }

    */
    #endregion Old Hand Movement Variables and Methods

}
