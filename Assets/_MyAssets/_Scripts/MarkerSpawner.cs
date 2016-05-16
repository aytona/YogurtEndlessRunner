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

	[Tooltip("The distance the flag spawns on every..\tNeeds to be min 32m")]
	public int repeatDistance;

	[Tooltip("Player object")]
	public Transform player;

	/// <summary>
	/// The counter of flags spawned
	/// </summary>
	private int counter = 1;

	private float offset;
	private float nextDistance;

	private bool setOffset = false;
	private bool flagSpawned = false;

	void Start() {
		nextDistance = repeatDistance * counter;
		if (player == null) {
			player = FindObjectOfType<PlayerMovement>().parentObject.transform;
		}
	}

	void Update() {
		if (GameManager.Instance.gameSettings.gameStart && !setOffset) {
			offset = transform.position.x - player.transform.position.x;

			setOffset = true;
			flagSpawned = false;
		}

		if (GameManager.Instance.gameSettings.gameRestart && setOffset) {
			setOffset = false;
		}

		else if (offset > 0 && !flagSpawned &&
			(int)(GameManager.Instance.gameSettings.distance + offset) % (int)(nextDistance) == 0) {
                Debug.Log("Spawning Flag");
			    SpawnFlag();
		}
	}

	private void SpawnFlag() {
		flagSpawned = true;
		nextDistance += repeatDistance;
		counter++;
		GameObject flag = Instantiate(_flagPrefab, transform.position, Quaternion.identity) as GameObject;
		flag.GetComponent<DistanceMarker>().distanceText = (repeatDistance * (counter - 1)).ToString();
		flagSpawned = false;
	}
}
