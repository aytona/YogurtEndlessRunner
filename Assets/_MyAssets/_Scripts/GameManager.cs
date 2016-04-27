using UnityEngine;
using System.Collections;

public enum DeviceAspect{
    iPhone4,        // 3:2
    iPhone5,        // 16:9
    iPad,           // 4:3
    Unknown, 
    DeviceAspectCount
}

// Everything that needs to be a single should be put in here
public class GameManager : Singleton<GameManager> {

    [Tooltip("Script that mostly handles the state of the game")]
    public GameSettings gameSettings;

    [Tooltip("Script that handles all saves")]
    public Data gameData;

    [Tooltip("The x-position of where objects get despawned")]
    public float lengthBeforeDespawn;

    /// <summary>
    /// The aspect ratio of device the game is currently being played on.  Only updated on Awake.
    /// </summary>
    public DeviceAspect currentAspect { get { return aspect; } }
    private DeviceAspect aspect;

    // Used in editor
    private float iPhone4Aspect = 1.5f;
    private float iPhone5Aspect = 1.77f;
    private float iPadAspect = 1.33f;

#if UNITY_EDITOR

    void Awake()
    {
        if (Camera.main.aspect >= iPhone5Aspect)
        {
            // 16:9
            aspect = DeviceAspect.iPhone5;
        }
        else if (Camera.main.aspect >= iPhone4Aspect)
        {
            // 3:2
            aspect = DeviceAspect.iPhone4;
        }
        else if (Camera.main.aspect >= iPadAspect)
        {
            // 4:3
            aspect = DeviceAspect.iPad;
        }
        else
        {
            // Unknown
            Debug.Log("Device aspect ratio is unknown.  Aspect ratio is: " + Camera.main.aspect);
            aspect = DeviceAspect.Unknown;
        }
    }

#elif UNITY_IOS

    void Awake()
    {
        if (SystemInfo.deviceModel.Contains("Unknown"))
        {
            // Unknown iOS device
            Debug.Log("Device is unknown.  Aspect ratio is: " + Camera.main.aspect);
            aspect = DeviceAspect.Unknown;
        }
        else if (SystemInfo.deviceModel.Contains("iPad"))
        {
            // iPad or iPad mini device
            aspect = DeviceAspect.iPad;
        }
        else if (SystemInfo.deviceModel.Contains("5") || SystemInfo.deviceModel.Contains("6"))
        {
            // iPhone 5 or iPhone 6 device
            aspect = DeviceAspect.iPhone5;
        }
        else
        {
            // iPhone 4 or iPod Touch or older
            aspect = DeviceAspect.iPhone4;
        }
    }

#endif
}
