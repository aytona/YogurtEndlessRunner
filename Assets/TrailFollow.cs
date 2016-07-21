using UnityEngine;
using System.Collections;

public class TrailFollow : MonoBehaviour {

    public Transform player;
    public float dampTime = 0.15f;
    private LineRenderer trail;
    public float linePointDist = 3;
    private Vector3 velocity = Vector3.zero;

    public Vector3[] positions = new Vector3[5];

    public Transform hidePoint;
    public bool hideTrail = true;

	void Start () 
    {
        trail = GetComponent<LineRenderer>();
	}
	
	void Update () 
    {
        if (hideTrail)
        {
            //for (int i = 0; i < positions.Length; i++)
            //{
                positions[0] = hidePoint.position;
            //}
        }
        else
        {
            positions[0] = player.position; // Follow the player
            positions[0].y += 0.2f;
        }
        trail.SetPosition(4, positions[0]);   // Lead line point at the player's position always.
        CalculateLinePoints();
	}

    // Calculate the new points for the trail line
    private void CalculateLinePoints()
    {
        for(int i = 1; i < positions.Length; i++)
        {
            Vector3 delta = positions[i - 1] - positions[i];
            delta.x -= linePointDist;//= positions[i].x;
            //delta.z = positions[i].z;
            Vector3 destination = positions[i] + delta;
            destination.x -= linePointDist;//= positions[i].x;
            //destination.z = positions[i].z;
            positions[i] = Vector3.SmoothDamp(positions[i], destination, ref velocity, dampTime);
        }
        SetLinePoints();
    }

    // Set the new points
    private void SetLinePoints()
    {
        trail.SetPosition(0, positions[4]);
        trail.SetPosition(1, positions[3]);
        trail.SetPosition(2, positions[2]);
        trail.SetPosition(3, positions[1]);
    }
}
