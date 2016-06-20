using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour {

    #region SettingsCanvas
    [Header("Settings Canvas")]
    public GameObject soundButton;
    public GameObject adsButton;

    // Order of sprites is important
    [Tooltip("0 is On, 1 is Off")]
    public Sprite[] soundSprites;
    public Sprite[] adsSprites;

    private bool soundToggle = true;
    private bool adsToggle = true;

    public void ToggleSound()
    {
        soundToggle = !soundToggle;
    }

    public void ToggleAds()
    {
        adsToggle = !adsToggle;
    }
    #endregion

    // TODO: Find a way to update sprites outside of update
    void Update()
    {
        soundButton.GetComponent<Button>().image.overrideSprite = soundSprites[Convert.ToInt32(soundToggle)];
        adsButton.GetComponent<Button>().image.overrideSprite = adsSprites[Convert.ToInt32(adsToggle)];
        AudioListener.pause = !soundToggle;
    }
}