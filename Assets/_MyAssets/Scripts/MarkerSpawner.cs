using UnityEngine;
using System.Collections;

public class MarkerSpawner : MonoBehaviour {

	[Tooltip("The prefab of the flag")]
	public GameObject _flagPrefab;

    [Tooltip("The prefab of the best distance flag")]
    public GameObject _bestFlagPrefab;

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
    private float bestDistance;

    private bool bestDistanceSpawned;

    private float iPadOffset = 4.125f;
    private float iPhone4Offset = 4.305f;
    private float iPhone5Offset = 4.535f;

	void Start() {
        counter = 1;
        repeatDistance = 25;                // Do we want this manually set like this?
		nextDistance = (repeatDistance - offset) * counter;
        bestDistance = GameManager.Instance.gameData.GetBestDistance();
        bestDistanceSpawned = false;
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
        if (nextDistance - GameManager.Instance.gameSettings.distance < 1.0f + offset) { // Should prevent multiple spawns at once
            SpawnFlag();
        }
        if ((bestDistance > 0f && bestDistance - GameManager.Instance.gameSettings.distance < 1.0f + offset)
            && !bestDistanceSpawned) {
            SpawnBestFlag();
        }
	}

	private void SpawnFlag() {
		nextDistance += repeatDistance;
		counter++;
		GameObject flag = Instantiate(_flagPrefab, transform.position, transform.rotation) as GameObject;   // The platform tilt seems to be around 54 degree on X
		flag.GetComponent<DistanceMarker>().distanceText = (repeatDistance * (counter - 1)).ToString() + "M";
	}

    private void SpawnBestFlag() {
        bestDistanceSpawned = true;
        GameObject flag = Instantiate(_bestFlagPrefab, transform.position, transform.rotation) as GameObject;
        flag.GetComponent<DistanceMarker>().distanceText = (bestDistance.ToString("F2") + "M");
    }
}
