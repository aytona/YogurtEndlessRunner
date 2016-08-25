using UnityEngine;
using System.Collections;

public class DistanceMarker : MonoBehaviour {

	public string distanceText;

	void Start() {
		GetComponentInChildren<TextMesh>().text = distanceText;
	}

	void Update() {
		transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);

		if (transform.position.x <= GameManager.Instance.lengthBeforeDespawn || 
			!GameManager.Instance.gameSettings.gameStart)
		{
			Destroy(gameObject);
		}
	}
}