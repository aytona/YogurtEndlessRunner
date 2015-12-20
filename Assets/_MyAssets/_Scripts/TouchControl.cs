using UnityEngine;
using System.Collections;

public enum SwipeDirection {up, down, left, right};

public class TouchControl : MonoBehaviour {
    private Vector2 startPos;
    private Vector2 direction;
    public bool directionChosen;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DetectSwipe();
	}

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    Debug.Log("Touch");
                    break;
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Ended:
                    directionChosen = true;
                    break;
            }
        }
        if (directionChosen)
        {
            if (direction.y > 0 && direction.y > direction.x)
            {
                Debug.Log("Swipe Up");
            }
            if (direction.y < 0 && direction.y < direction.x)
            {
                Debug.Log("Swipe Down");
            }
            if (direction.x > 0 && direction.x > direction.y)
            {
                Debug.Log("Swipe Right");
            }
            if (direction.x < 0 && direction.x < direction.y)
            {
                Debug.Log("Swipe Left");
            }
        }
    }
}
