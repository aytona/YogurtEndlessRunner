using UnityEngine;
using System.Collections;

public class DistanceMarker : MonoBehaviour {

	public string distanceText;

	void Start() {
		distanceText = GetComponentInChildren<TextMesh>().text;
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