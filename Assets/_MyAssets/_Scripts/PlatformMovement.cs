/* Deprecated Script
 * Check BackgroundManager.cs
 * TODO: Scheduled for deletion
 */

using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    #region Variables

    [Tooltip("Width of the platform")]
    public float widthOfPlatform;

    #endregion Variables

    #region Monobehaviour

    void FixedUpdate()
    {
        // Moves the platform to the left
        if (transform.position.x >= GameManager.Instance.lengthBeforeDespawn)
            transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
        // Relocate the platform to the front
        else
            transform.Translate(widthOfPlatform, 0, 0);
    }

    #endregion Monobehavious
}
