using UnityEngine;
using System.Collections;

// Everything that needs to be a single should be put in here
public class GameManager : Singleton<GameManager> {

    [Tooltip("Script that mostly handles the state of the game")]
    public GameSettings gameSettings;

    [Tooltip("The x-position of where objects get despawned")]
    public float lengthBeforeDespawn;
}
