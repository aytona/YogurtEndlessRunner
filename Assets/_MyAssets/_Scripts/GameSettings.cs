using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    [Tooltip("The speed of the objects")]
    public float gameSpeed;

    [Tooltip("The weight of the player")]
    public float playerWeight;

}
