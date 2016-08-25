using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

    public Vector3 rotationAxis;
    public float rotationSpeed = 1.0f;

	void Update () {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
	}
}
