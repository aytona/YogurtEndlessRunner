using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {

    #region Variables

    [Tooltip("Speed of the platform")]
    public float speed;

    [Tooltip("Width of the platform")]
    public float widthOfPlatform;

    [Tooltip("Position of where the platform starts to despawn")]
    public float lengthBeforeDespawn;

    #endregion Variables

    #region Monobehaviour

    void FixedUpdate()
    {
        // Moves the platform to the left
        if (transform.position.x >= lengthBeforeDespawn)
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        // Relocate the platform to the front
        else
            transform.Translate(widthOfPlatform, 0, 0);
    }

    #endregion Monobehavious
}
