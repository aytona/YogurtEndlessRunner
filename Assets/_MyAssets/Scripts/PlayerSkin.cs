using UnityEngine;
using System.Collections;

public class PlayerSkin : MonoBehaviour {

    public SkinnedMeshRenderer playerMesh;

    public Material[] skin;

	// Use this for initialization
	void Start () 
    {
        playerMesh.material = skin[CharacterSelectScreen.Instance.GetSelectedCharNum()];
	}
}
