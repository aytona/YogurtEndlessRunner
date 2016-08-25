using UnityEngine;
using System.Collections;

public class TrailSpawner : MonoBehaviour {
    /*
    //public float spawnIntervalRatio = 1.0f;  // Add this later if necessary
    [Tooltip("The sprites needed to create the trail. In order.")]
    public Sprite[] trailSprites;
    [Tooltip("The Prefab for the trail piece.")]
    public GameObject trailPiece;
    // Stores the trail coroutine to be stopped later.
    private Coroutine trailLoop;
    // The index of the sprite we are currently at.
    private int spriteCount = 0;
    // The speed to send the trail pieces.
    private float constantSpeed = 1.0f;

    void Start()
    {
        constantSpeed = GameManager.Instance.gameSettings.gameSpeed;    // Set the constant speed at the start of the game.
    }

    /// <summary>
    /// Start showing a trail behingf the player.
    /// </summary>
    public void StartTrail()
    {
        trailLoop = StartCoroutine(SpawnTrailLoop());
    }

    /// <summary>
    /// Stop showing the trail behind the player.
    /// </summary>
    public void StopTrail()
    {
        StopCoroutine(trailLoop);
        spriteCount = 0;
    }

    private IEnumerator SpawnTrailLoop()
    {
        // Spawn a trail piece
        GameObject piece = Instantiate(trailPiece, transform.position, transform.rotation) as GameObject;
        // Set the sprite on the trail piece
        piece.GetComponent<TrailPiece>().SetTrail(trailSprites[spriteCount], 0.5f, constantSpeed);

        // Attached trail:
        piece.transform.parent = this.transform;
        piece.transform.localScale = Vector3.one;
        
        // Detatched trail:
        //piece.transform.localScale = transform.localScale;

        // Count up
        spriteCount++;
        // If the count is at the end of the sprite array start back at the start
        if(spriteCount >= trailSprites.Length)
        {
            spriteCount = 0;
        }

        // Wait for the amout of time to have all the frames displayed in a second.
        yield return new WaitForSeconds(1.0f/trailSprites.Length);  // This may need to change if we want the detached trail to be smoother.

        // Restart the loop
        trailLoop = StartCoroutine(SpawnTrailLoop());
    }
     */

    private LineRenderer trail;
    public float scrollSpeed = 0.01f;
    private bool trailOn = false;
    private TrailFollow follow;

    void Start()
    {
        trail = GetComponent<LineRenderer>();
        follow = GetComponent<TrailFollow>();
    }

    void Update()
    {
        if(trailOn)
            AnimateTrail();
    }

    private void AnimateTrail()
    {
        float offset = trail.material.mainTextureOffset.x + Time.deltaTime * scrollSpeed;
        trail.material.mainTextureOffset = new Vector2(offset, 0);
    }

    public void StartTrail()
    {
        trailOn = true;
        follow.hideTrail = false;
    }

    public void StopTrail()
    {
        trailOn = false;
        follow.hideTrail = true;
        trail.material.mainTextureOffset = Vector2.zero;
    }
}
