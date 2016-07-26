using UnityEngine;
using System.Collections;

public class WarningSignPosition : MonoBehaviour {

    public float xOffset;

    void Start()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Vector3 destination = new Vector3(worldPoint.x - xOffset, 0, 0);
        gameObject.transform.Translate(destination);
    }
}
