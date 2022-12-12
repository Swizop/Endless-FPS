using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCanvas;

    [Header("Volume")]
    [SerializeField]
    private TMP_Text volumeTextValue = null;                // the volume label text value should change based on the slider value
    [SerializeField]
    private Slider volumeSlider = null;
    private float volumeBeforeChange;

    private void Start()
    {
//        Debug.Log("SettingsController Start was called");
        volumeBeforeChange = AudioListener.volume;
        volumeSlider.value= volumeBeforeChange;
        volumeTextValue.text = volumeBeforeChange.ToString("0.0");
    }

    public void SetVolume(float volume)
    {
        volumeTextValue.text = volume.ToString("0.0");
    }

    // this method saves all the changes to PlayerPrefs
    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        AudioListener.volume = volumeSlider.value;
        volumeBeforeChange= AudioListener.volume;
        Debug.Log("Settings applied");
    }

    public void RestoreSettings()
    {
        volumeTextValue.text = volumeBeforeChange.ToString("0.0");
        volumeSlider.value = volumeBeforeChange;
    }
}
