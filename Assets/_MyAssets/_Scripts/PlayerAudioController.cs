using UnityEngine;
using System.Collections;

public class PlayerAudioController : MonoBehaviour {

    public AudioClip[] audioClips;
    private AudioSource _aSource;

    void Start(){
        _aSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipNum)
    {
        _aSource.clip = audioClips[clipNum];
        _aSource.Play();
    }

}
