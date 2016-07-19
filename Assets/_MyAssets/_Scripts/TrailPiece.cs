using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class TrailPiece : MonoBehaviour {

    [Tooltip("The sprite renderer component of the trail piece.")]
    public SpriteRenderer sRenderer;
    private float movementSpeedFactor = 1.0f;   // Set this to public if needed later
    // Saves the speed at which the piece will travel.
    private float constantSpeed = 1.0f;

	void Start () 
    {
        //s = GetComponent<SpriteRenderer>(); // This doesn't seem to work, so I do it manually on the prefab.
	}

    void Update()
    {
        // Move
        transform.Translate(Vector3.left * Time.deltaTime * constantSpeed * movementSpeedFactor);
    }
	
    /// <summary>
    /// Set the trail going behing the player.
    /// </summary>
    /// <param name="newSprite">The sprite to display on the trail piece spawend.</param>
    /// <param name="destroyTime">The time which the piece will destroy itself after spawning.</param>
    /// <param name="moveSpeed">The speed at which the piece will move.</param>
    public void SetTrail(Sprite newSprite, float destroyTime, float moveSpeed)
    {
        sRenderer.sprite = newSprite;
        constantSpeed = moveSpeed;
        StartCoroutine(DestroyInTime(destroyTime));
    }

    // Destroys the piece after it is spawned.
    private IEnumerator DestroyInTime(float timeToDestruction)
    {
        yield return new WaitForSeconds(timeToDestruction);
        Destroy(this.gameObject);
    }
}
