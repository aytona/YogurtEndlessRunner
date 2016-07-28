using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] ContinousBackground;
    public GameObject[] ChangingBackground;
    public float widthOfPlatform;
    public float distBetweenChange;

    void FixedUpdate()
    {
        ContiniousMovement();
    }

    private void ContiniousMovement()
    {
        foreach(GameObject i in ContinousBackground)
        {
            if (i.transform.position.x >= GameManager.Instance.lengthBeforeDespawn)
                i.transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
            else
                i.transform.Translate(widthOfPlatform, 0, 0);
        }
    }
}

