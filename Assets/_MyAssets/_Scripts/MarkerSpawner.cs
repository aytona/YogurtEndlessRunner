using UnityEngine;
using System.Collections;

public class MarkerSpawner : MonoBehaviour {

	// Average Speed: s = d/t
	// Known: Speed of flag, and distance it needs to travel (s and d)
	// Find: Total time (t)
	// Time formula: t = d/s
	// Total distance = markerspaner.x - player.x
	// Speed = gamesettings.gamespeed (need average speed)

	[Tooltip("The prefab of the flag")]
	public GameObject _flagPrefab;

	[Tooltip("The distance the flag spawns on every..")]
	public int repeatDistance;

	[Tooltip("Player object")]
	public Transform player;

	/// <summary>
	/// The counter of flags spawned
	/// </summary>
	private int counter = 1;

	void Update() {
//		if ((GameManager.Instance.gameSettings.distance /* - offset */) % repeatDistance == 0 &&
//			GameManager.Instance.gameSettings.gameStart) {
//
//		}

		if (GameManager.Instance.gameSettings.distance == ((gameObject.transform.position.x - player.transform.position.x) * counter)) {
			GameObject flag = Instantiate(_flagPrefab);
			flag.GetComponent<DistanceMarker>().distanceText = (repeatDistance * counter).ToString();
		}
	}
}
