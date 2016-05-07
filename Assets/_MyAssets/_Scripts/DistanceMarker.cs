using UnityEngine;
using System.Collections;

public class DistanceMarker : MonoBehaviour {

	private Renderer rend;
	private Material mat;

	void Start() {
		rend = GetComponent<Renderer>();
		mat = rend.material;
	}

	void Update() {
		float emission = Mathf.PingPong(Time.time, 0.4f);
		Color baseColor = Color.white;
		Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
		mat.SetColor("_EmissionColor", finalColor);
	}
}
