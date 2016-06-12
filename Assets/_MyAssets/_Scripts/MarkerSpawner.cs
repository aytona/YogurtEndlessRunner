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

    private float iPadOffset = 4.125f;
    private float iPhone4Offset = 4.305f;
    private float iPhone5Offset = 4.535f;

	void Start() {
        counter = 1;
        repeatDistance = 25;
		nextDistance = (repeatDistance - offset) * counter;
		if (player == null) {
			player = FindObjectOfType<PlayerMovement>().parentObject.transform;
		}
        switch(GameManager.Instance.currentAspect)
        {
            case DeviceAspect.iPad:
                offset = iPadOffset;
                break;
            case DeviceAspect.iPhone4:
                offset = iPhone4Offset;
                break;
            case DeviceAspect.iPhone5:
                offset = iPhone5Offset;
                break;
        }
	}

	void Update() {
		/*if (GameManager.Instance.gameSettings.gameStart && !setOffset) {
			offset = transform.position.x - player.transform.position.x;

			setOffset = true;
			flagSpawned = false;
		}*/

        if(!setOffset)
        {
            setOffset = true;
            flagSpawned = false;
        }

		/*if (GameManager.Instance.gameSettings.gameRestart && setOffset) {
			setOffset = false;
		}*/

		else if (offset > 0 && !flagSpawned &&
			(int)(GameManager.Instance.gameSettings.distance + offset) % (int)(nextDistance) == 0) {
                if (nextDistance - GameManager.Instance.gameSettings.distance < 1.0f + offset) // Should prevent multiple spawns at once
                {
                    //Debug.Log("Spawning Flag");
                    SpawnFlag();
                }
		}
	}

	private void SpawnFlag() {
		flagSpawned = true;
		nextDistance += repeatDistance;
		counter++;
		GameObject flag = Instantiate(_flagPrefab, transform.position, transform.rotation) as GameObject;   // The platform tilt seems to be around 54 degree on X
		flag.GetComponent<DistanceMarker>().distanceText = (repeatDistance * (counter - 1)).ToString() + "M";
		flagSpawned = false;

        // Debug.Log(GameManager.Instance.gameSettings.distance);
        // Debug.Log(GameManager.Instance.gameSettings.distance + offset);
	}
}
