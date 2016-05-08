using UnityEngine;
using System.Collections;

public class TesterMarker : MonoBehaviour {

    private bool go = false;
    private float startDistance = 0;
    private float endDistance = 0;
    private float startTime = 0;
    private float endTime = 0;

    public Transform endPoint;

    public Transform spawnPoint;
	
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.G))
        {
            go = true;
            StartStatCounting();
        }
        if(go)
        {
            transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
        }

        if(transform.position.x <= endPoint.position.x)
        {
            if(go)
                StopCounting();
        }
	}

    /*void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        if(other.CompareTag("endFlag"))
        {
            Debug.Log("Stopping counting");
            StopCounting();
        }
    }*/

    void StartStatCounting()
    {
        transform.position = spawnPoint.position;
        startDistance = GameManager.Instance.gameSettings.distance;
        startTime = Time.time;
    }

    void StopCounting()
    {
        endDistance = GameManager.Instance.gameSettings.distance;
        endTime = Time.time;
        DisplayStats();
        go = false;
    }

    void DisplayStats()
    {
        string msg = "Start distance: " + startDistance + "\n" +
                        "End distance: " + endDistance + "\n" +
                        "Start Time: " + startTime + "\n" +
                        "End Time: " + endTime + "\n";

        Debug.Log(msg);
    }
}
