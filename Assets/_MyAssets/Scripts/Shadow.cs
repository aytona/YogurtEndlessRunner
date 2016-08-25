using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {

    public Transform player;
   // public float yOffset, zOffset;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        newPosition.x = player.position.x;
        transform.position = newPosition;
	}
}
