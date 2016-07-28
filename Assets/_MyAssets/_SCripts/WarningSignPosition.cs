using UnityEngine;

public class WarningSignPosition : MonoBehaviour {

    public float xOffsetPercent;
    public TextMesh textMesh;

    void Start()
    {
        float xOffset = (xOffsetPercent / 100) * Screen.width;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Vector3 destination = new Vector3(worldPoint.x - xOffset, 0, 0);
        gameObject.transform.Translate(destination);
        textMesh = gameObject.GetComponent<TextMesh>();
    }
}
