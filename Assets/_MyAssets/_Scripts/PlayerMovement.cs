using UnityEngine;
using System.Collections;

// The GameObject requires a Rigidbody component
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Enums

    public enum State
    {
        Normal,
        Hit, 
        TwoHit,
        EndGame,
        None
    }

    #endregion Enums

    #region Extra Classes

    [System.Serializable]
    public class TransformArray2D
    {
        public Transform[] targetArray;
    }

    #endregion Extra Classes

    #region Variables

    [Tooltip("Array of targets, where the player can move to, don't mess with this.")]
    /// <summary>
    /// Array of targets, where the player can move to.
    /// </summary>
    [SerializeField]
    private TransformArray2D[] targets = null;

    /// <summary>
    /// Device the game is currently being played on. 0 - iPhone4, 1 - iPhone5, 2 - iPad.
    /// </summary>
    private int currentDevice = 2;

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

    [Tooltip("Maximum force at which the player will jump, affects height.")]
    /// <summary>
    /// Maximum force of the character's jump.
    /// </summary>
    public float maxJumpForce = 1.0f;

    [Tooltip("Minimum force at which the player will jump, affects height.")]
    /// <summary>
    /// Minimum force of the character's jump.
    /// </summary>
    public float minJumpForce = 1.0f;

    /// <summary>
    /// Is the player on the ground, can he jump?
    /// </summary>
    [SerializeField]
    private bool isGrounded = false;

    /// <summary>
    /// Used for jumping physics math.
    /// </summary>
    private float savedJumpForce = 1.0f;

    /// <summary>
    /// If the player is jumping.
    /// </summary>
    private bool isJumping = false;

    /// <summary>
    /// If the player is falling down.
    /// </summary>
    private bool isFallingDown = false;

    /// <summary>
    /// Player's rigidbody.
    /// </summary>
    private Rigidbody m_RigidBody;

    /// <summary>
    /// Is the game over?
    /// </summary>
    private bool gameOver = true;       // Start as true to wait for start of game.

    /// <summary>
    /// Is it the start of the game?  Used to set stuff in motion after the player presses start.
    /// </summary>
    private bool startOfGame = true;

    [Tooltip("The parent object of the player.  The object that will move between planes. DO NOT MESS WITH UNLESS YOU KNOW WHAT IT DOES!")]
    /// <summary>
    /// Container of the player object.
    /// </summary>
    public Transform parentObject;

    /// <summary>
    /// Layer mask for the racasting to detect what plane the player wants to move to.
    /// </summary>
    private int layerMask = 1 << 8;

    [Tooltip("Maximum gravity the character will have when falling down. Manipulate the Y value (negative is down).")]
    /// <summary>
    /// Maximum gravity of character when falling down.
    /// </summary>
    public Vector3 MaxGravity;

    [Tooltip("Minimum gravity the character will have when falling down. Manipulate the Y value (negative is down).")]
    /// <summary>
    /// Minimum gravity of character when falling down.
    /// </summary>
    public Vector3 MinGravity;

    [Tooltip("Regular gravity, y = -9.81.")]
    /// <summary>
    /// Regular Gravity.
    /// </summary>
    public Vector3 regularGravity;

    [Tooltip("Time the character will stay in the air when it jumps. (in seconds)")]
    /// <summary>
    /// Time the character will stay in the air when it jumps.
    /// </summary>
    public float timeInTheAir;

    [Tooltip("Distance the player will move forward in its jump. Creates the jump arc.")]
    /// <summary>
    /// Distance the player will move forward in its jump
    /// </summary>
    public float jumpDistance = 1.0f;

    [Tooltip("Reference to the shadow sprite.  Used to fade out the shadow.")]
    /// <summary>
    /// Reference to the shadow sprite.  Used to fade out the shadow.
    /// </summary>
    public SpriteRenderer shadowSprite;

    [Tooltip("Time the character surf on the spoon. (in seconds)")]
    /// <summary>
    /// Amount of time the surfing will last.
    /// </summary>
    public float surfTime = 10.0f;

    /// <summary>
    /// Stores the surfing coroutine.
    /// </summary>
    private Coroutine surfCoroutine = null;

    /// <summary>
    /// If the player is surfing.
    /// </summary>
    private bool isSurfing = false;

    [Tooltip("The duration of how long the player will blink")]
    /// <summary>
    /// The duration of how long the blinking effect will last
    /// </summary>
    public float blinkDuration = 1f;

    [Tooltip("The delay between each blink")]
    /// <summary>
    /// The duration between each blink
    /// </summary>
    public float blinkTime = 0.01f;

    /// <summary>
    /// Check to see if the player is already in blinking effect
    /// </summary>
    private bool inBlink = false;

    /// <summary>
    /// Stores the blink coroutine.
    /// </summary>
    private Coroutine blinkCoroutine = null;

    /// <summary>
    /// Used for blinking the player.
    /// </summary>
    public SkinnedMeshRenderer m_Renderder;

    /// <summary>
    /// Current state of the player
    /// </summary>
    public State m_CurrentState;

    /// <summary>
    /// Stores the state coroutine.
    /// </summary>
    private Coroutine stateCoroutine = null;

    /// <summary>
    /// Length of time for recovery
    /// </summary>
    public float recoveryDelay;

    /// <summary>
    /// Checker if the speed has already been modified, so it doesn't stack
    /// </summary>
    private bool speedModifierChecker = false;

    /// <summary>
    /// Length of time for the speed modifier to last
    /// </summary>
    private float speedModifierDuration = 0;

    /// <summary>
    /// The amount the speed is modified by in decimal
    /// </summary>
    private float speedModifierAmount = 0.75f;  // TODO: Assign this number on the object if there are other objects that modifies the speed

    /// <summary>
    /// Reference to the class that controls all the player's animations.
    /// </summary>
    private PlayerAnimation m_Animations;

    /// <summary>
    /// Reference to the class that controls all the player's audio.
    /// </summary>
    private PlayerAudioController m_PlayerAudio;

    /// <summary>
    /// Reference to the game controller.
    /// </summary>
    private GameController m_GameController;
    private Game m_GC;

    public EffectController effectController;

    public TrailSpawner trail;

    #endregion Variables

    #region Monobehaviour

    void Start()
    {
        // Get all references needed
        m_RigidBody = GetComponent<Rigidbody>();
        m_GameController = FindObjectOfType<GameController>();
        m_GC = FindObjectOfType<Game>();
        //m_Renderder = GetComponentInChildren<SkinnedMeshRenderer>();  
        m_Animations = GetComponentInChildren<PlayerAnimation>();
        m_PlayerAudio = GetComponent<PlayerAudioController>();

        //effectController = GetComponentInChildren<EffectController>();

        // Get the current device.  Sets the positions for the hand to move to and from
        currentDevice = (int)GameManager.Instance.currentAspect;
        if (currentDevice > 2)
        {
            currentDevice = 2;
        }
    }

	void Update () {

        if (!gameOver)
        {
            if(startOfGame)
            {
                // Start the game at 1 hit this resembles subway surfers
                if(stateCoroutine != null)
                {
                    StopCoroutine(stateCoroutine);
                }
                stateCoroutine = StartCoroutine(StateRecovery(recoveryDelay));
                startOfGame = false;
            }
            CheckInput();       // For testing
            MoveToPosition();   // Constantly move to the next position, the next position is changed by the touch control
            ResetGravity();     // Checks if the player is grounded and resets the gravity to normal if grounded
        }

        // If the game is over reset the gravity to regular gravity
        if (gameOver)
        {
            Physics.gravity = regularGravity;
        }

	}

    void FixedUpdate()
    {
        if (isJumping)
        {
            FixedUpdateJump();

            if (!isSurfing)
            {
                if (Random.Range(0, 20) < 15)
                    m_Animations.Play(PlayerAnimation.PlayerStates.Jump);
                else
                    m_Animations.Play(PlayerAnimation.PlayerStates.Jump2);
            }
            else
            {
                m_Animations.Play(PlayerAnimation.PlayerStates.Jump);
            }
            //Debug.Log("Jumping");
            isJumping = false;
        }
        if (isFallingDown)
        {
            ChangeGravity();
            DownForce();

            isFallingDown = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) // Check if player is grounded before player can jump again.
        {  
            isGrounded = true;
            m_Animations.Play(PlayerAnimation.PlayerStates.Grounded);
            effectController.Impact(); ;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))             // Check if player has arrived at target.
            arrivedAtTarget = true;

        if (other.CompareTag("Hand"))               // If the hand has grabbed the player.  The GAME IS OVER!
        {
            gameOver = true;
            GameManager.Instance.gameSettings.gameStart = false;
            GameManager.Instance.gameSettings.ZeroSpeed();
            HandMovement hand = other.gameObject.GetComponentInParent<HandMovement>();
            transform.parent.position = hand.GetGrabPosition().position;
            hand.SetGrabAnim();
            hand.SetGameOver(true);
            m_Animations.Play(PlayerAnimation.PlayerStates.Caught);
            m_PlayerAudio.PlaySound(3);
            AttachToHand(hand.yogurtGrabPos);
            StartCoroutine(FadeOutSprite(shadowSprite));
            m_CurrentState = State.EndGame;
            if (m_GameController != null)
                m_GameController.SetGameOver(true);
            else if (m_GC != null)
                m_GC.SetGameOver(true);
        }

        if (other.CompareTag("Collectable"))        // Pick up a collectable that gives you points
        {
            //Debug.Log("CANDY!");
            effectController.Collected();
            if (m_GameController != null)
                m_GameController.IncrementScore(125);   // Increment score [NOTE: there is a better way to do this.]
            else if (m_GC != null)
                m_GC.IncrementScore(125);
            m_PlayerAudio.PlaySound(1);
        }

        if (other.CompareTag("Obstacle"))           // Hit an obstacle
        {
            effectController.Hit();
            //Debug.Log("OBSTACLE!");
            m_PlayerAudio.PlaySound(2);
            GetHit();
        }

        if (other.CompareTag("ObstacleLarge"))           // Hit a large obstacle
        {
            effectController.Hit();
            //Debug.Log("OBSTACLE!");
            m_PlayerAudio.PlaySound(2);
            GetBigHit();
        }

        if (other.CompareTag("SpeedModifier"))      // Collectable that slows down the speed
        {
            if (!speedModifierChecker)
            {
                StartCoroutine(SpeedModifierEffect(speedModifierDuration, speedModifierAmount));
            }
        }

        if(other.CompareTag("Spoon"))
        {
            Surf(true);
        }

        if (other.CompareTag("JumpHeight"))         // If the player hits the "ceiling" of it's jump
        {
            isFallingDown = true;
        }

    }

    #endregion Monobehaviour

    #region Public Methods

    /// <summary>
    /// Moves the character up a plane.
    /// </summary>
    public void MoveUp()
    {
        if (!gameOver)
        {
            if (isGrounded && arrivedAtTarget)
            {
                if (targetIndex < (targets.Length - 1))
                {
                    targetIndex++;
                    arrivedAtTarget = false;
                }
                m_Animations.Play(PlayerAnimation.PlayerStates.LeftBump);
                //Debug.Log("Move Up");
            }
        }
    }

    /// <summary>
    /// Moves the character down a plane.
    /// </summary>
    public void MoveDown()
    {
        if (!gameOver)
        {
            if (isGrounded && arrivedAtTarget)
            {
                if (targetIndex > 0)
                {
                    targetIndex--;
                    arrivedAtTarget = false;
                }
                m_Animations.Play(PlayerAnimation.PlayerStates.RightBump);
                //Debug.Log("Move Down");
            }
        }
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    public void Jump()
    {
        isJumping = true;
        count = Time.time;
    }

    public void Surf(bool isSurfing)
    {
        if (isSurfing)
        {
            if (surfCoroutine != null)
            {
                StopCoroutine(surfCoroutine);
            }
            m_CurrentState = State.Normal; // [NOTE: This freezes the game] [NOTE AGAIN: Maybe not]
            surfCoroutine = StartCoroutine(StartSurfing());
        }
        else
        {
            if (surfCoroutine != null)
            {
                StopCoroutine(surfCoroutine);
            }
            DeactivateSurfing();
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
            transform.position = p.position;
            m_RigidBody.isKinematic = true;
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

    /// <summary>
    /// Set the game to be over.
    /// </summary>
    /// <param name="b">True = Game Over; False = Game Not Over.</param>
    public void SetGameOver(bool b)
    {
        gameOver = b;
    }

    /// <summary>
    /// See if the game over is true or not.
    /// </summary>
    /// <returns>Is game over?</returns>
    public bool GetGameOver()
    {
        return gameOver;
    }

    /// <summary>
    /// Function call whenever the player gets hit by an obstacle or anything that will hurt the player
    /// If there will ever be a life system in the game, add a float param that takes in the amount of damage
    /// the player will receive. But for now, its empty
    /// </summary>
    public void GetHit()
    {
        if (!isSurfing)
        {
            if (!inBlink)
            {
                if(blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(BlinkEffect(blinkDuration, blinkTime));
            }
            if (m_CurrentState == State.Hit)
            {
                m_CurrentState = State.TwoHit;
            }
            else if (m_CurrentState == State.Normal)
            {
                if(stateCoroutine != null)
                {
                    StopCoroutine(stateCoroutine);
                }
                stateCoroutine = StartCoroutine(StateRecovery(recoveryDelay));
            }
        }
        else
        {
            Surf(false);
        }
    }

    public void GetBigHit()
    {
        if (!isSurfing)
        {
            if (!inBlink)
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(BlinkEffect(blinkDuration, blinkTime));
            }
            m_CurrentState = State.TwoHit;
        }
        else
        {
            Surf(false);
            if (m_CurrentState == State.Normal)
            {
                if (stateCoroutine != null)
                {
                    StopCoroutine(stateCoroutine);
                }
                stateCoroutine = StartCoroutine(StateRecovery(recoveryDelay));
            }
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// For testing.  This changes the target index, which in turn changes where the player's next destination is.
    /// </summary>
    private void CheckInput()
    {
        if (m_CurrentState != State.TwoHit)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                MoveDown();
            if (Input.GetKeyDown(KeyCode.UpArrow))
                MoveUp();
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            if (Input.GetKeyDown(KeyCode.P))
                Surf(true);
            if (Input.GetKeyDown(KeyCode.O))
                Surf(false);
        }
    }

    float count = 0;

    /// <summary>
    /// Moves player from one point to another.
    /// </summary>
    private void MoveToPosition()
    {
        if (isGrounded)
        {
            //currentPos = this.transform.position;       // Set current position to the player's current postion
            currentPos = parentObject.position;
            nextPos = targets[currentDevice].targetArray[targetIndex].position;    // Sets the next position to one of the targets, indicated by the target index.

            // Interpolates the player's position between current position and destination.
            //transform.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * speed);
            if(Time.time - count < 0.65f)
                parentObject.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed * 0.5f); //speed * 0.2f);   // Slowly moving back after a jump
            else
                parentObject.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * speed);
        }
        else // Moving forward during a jump
        {
            //Debug.Log("Jump move");
            currentPos = parentObject.position;
            nextPos = new Vector3(targets[currentDevice].targetArray[targetIndex].position.x + jumpDistance, 
                                  targets[currentDevice].targetArray[targetIndex].position.y,
                                  targets[currentDevice].targetArray[targetIndex].position.z);    // Sets the next position to one of the targets, indicated by the target index.
            
            // Interpolates the player's position between current position and destination.
            parentObject.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * speed * 0.2f);
        }
    }

    private void FixedUpdateJump()
    {
        if (!gameOver)
        {
            if (isGrounded)
            {
                float jumpForce = maxJumpForce * (GameManager.Instance.gameSettings.playerWeight * 0.15f);
                if (jumpForce <= minJumpForce)
                    jumpForce = minJumpForce;
                else if (jumpForce > maxJumpForce)
                    jumpForce = maxJumpForce;
                //Debug.Log(jumpForce);
                //Debug.Log(GameManager.Instance.gameSettings.playerWeight);
                m_RigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                savedJumpForce = jumpForce;
                isGrounded = false;
                m_PlayerAudio.PlaySound(0);
                effectController.Jump();
            }
        }
    }

    /// <summary>
    /// Change gravity to fast falling gravity.
    /// </summary>
    private void ChangeGravity()
    {
        //rb.mass = fallingMass;
        Physics.gravity = regularGravity * GameManager.Instance.gameSettings.playerWeight;
        if (Physics.gravity.y < MaxGravity.y)
        {
            Physics.gravity = MaxGravity;
        }
        else if (Physics.gravity.y >= MinGravity.y)
        {
            Physics.gravity = MinGravity;
        }
    }

    /// <summary>
    /// Change gravity back to default gravity.
    /// </summary>
    private void ResetGravity()
    {
        //Debug.Log(Physics.gravity);
        if (isGrounded)
            Physics.gravity = regularGravity;
    }

    private void DownForce()
    {
        m_RigidBody.AddForce(-Vector3.up * (savedJumpForce/2), ForceMode.Impulse);
    }

    /// <summary>
    /// Activate the surfing animation.
    /// </summary>
    private void ActivateSurfing()
    {
        // Start trail
        trail.StartTrail();
        m_Animations.Play(PlayerAnimation.PlayerStates.Surf);
        if(!isSurfing)
            shadowSprite.transform.localScale = new Vector3(shadowSprite.transform.localScale.x * 2f, 
                                                            shadowSprite.transform.localScale.y, shadowSprite.transform.localScale.z);
        isSurfing = true;
    }

    /// <summary>
    /// Deactivate the surfing animation.
    /// </summary>
    private void DeactivateSurfing()
    {
        // Stop trail
        trail.StopTrail();
        m_Animations.Play(PlayerAnimation.PlayerStates.Run);
        isSurfing = false;
        shadowSprite.transform.localScale = new Vector3(shadowSprite.transform.localScale.x * 0.5f,
                                                        shadowSprite.transform.localScale.y, shadowSprite.transform.localScale.z);
    }

    private IEnumerator BlinkEffect(float duration, float delay)
    {
        inBlink = true;
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            m_Renderder.enabled = !m_Renderder.enabled;
            yield return new WaitForSeconds(delay);
        }
        m_Renderder.enabled = true;
        inBlink = false;
    }

    private IEnumerator StateRecovery(float recoveryDelay)
    {
        if (!isSurfing)
        {
            m_CurrentState = State.Hit;
            yield return new WaitForSeconds(recoveryDelay);
            if (m_CurrentState != State.TwoHit)
                m_CurrentState = State.Normal;
        }
    }

    private IEnumerator SpeedModifierEffect(float duration, float speed)
    {
        Time.timeScale = speed;
        speedModifierChecker = true;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
        speedModifierChecker = false;
    }

    private IEnumerator StartSurfing()
    {
        ActivateSurfing();
        yield return new WaitForSeconds(surfTime);
        DeactivateSurfing();
    }

    private IEnumerator FadeOutSprite(SpriteRenderer sr)
    {
        sr.color = new Color(0f, 0f, 0f, Mathf.SmoothStep(0.0f, sr.color.a, 0.6f));
        yield return new WaitForEndOfFrame();
        if(sr.color.a > 0.0f)
            StartCoroutine(FadeOutSprite(sr));
    }

    #endregion Private Methods

    #region Obsolete

    /// <summary>
    /// Check to see if you hit a plane and then change the index to the plane that was hit.
    /// [NOTE: No longer being used as there is no more direct lane selection]
    /// </summary>
    /// <param name="_t"></param>
    public void MovePlayerToPlane(Touch _t)
    {
        if (isGrounded && arrivedAtTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(_t.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                switch (hit.collider.name)
                {
                    case "FarPlane":
                        targetIndex = 2;
                        break;
                    case "MiddlePlane":
                        targetIndex = 1;
                        break;
                    case "NearPlane":
                        targetIndex = 0;
                        break;
                    default:
                        //Debug.Log("No Plane");
                        break;
                }
            }
        }
    }

    private void MovePlayerToPlane()
    {  
        // The following is for testing with the mouse
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                switch (hit.collider.name)
                {
                    case "FarPlane":
                        targetIndex = 2;
                        break;
                    case "MiddlePlane":
                        targetIndex = 1;
                        break;
                    case "NearPlane":
                        targetIndex = 0;
                        break;
                    default:
                        Debug.Log("No Plane");
                        break;
                }
            }
        }
    }


    #endregion Obsolete
}
