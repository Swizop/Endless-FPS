using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PauseSettingsController : MonoBehaviour
{
    // default volume value used for restoring the settings
    [Header("Volume")]
    [SerializeField] private float defaultVolume = 1.0f;

    // the volume label text value should change based on the slider value
    [SerializeField] private TMP_Text volumeTextValue = null;
    
    // volume slider object
    [SerializeField] private Slider volumeSlider = null;

    // variable used for reverting to the old volume value in case the settings are not saved
    private float volumeBeforeChange;


    [Header("GraphicsQuality")]
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    private int _qualityLevel;
    private int defaultQualityIndex;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private int defaultResolutionIndex;
    private int _resolutionIndex;
    private Resolution[] resolutions;


    private void Start()
    {
    // VOLUME
        // when the settings menu is instantiated, the game volume value is stored
        // so that the slider can be updated to the actual volume
        volumeBeforeChange = AudioListener.volume;
        volumeSlider.value= volumeBeforeChange;
        volumeTextValue.text = volumeBeforeChange.ToString("0.0");

    // RESOLUTIONS
        // initialize resolutions array with available screen resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options= new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // the default resolution option will always be the maximum one
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        defaultResolutionIndex = currentResolutionIndex;
        _resolutionIndex = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // QUALITY
        if (PlayerPrefs.HasKey("masterQuality"))
        {
            _qualityLevel = PlayerPrefs.GetInt("masterQuality");
        } 
        else
        {
            _qualityLevel = QualitySettings.GetQualityLevel();
        }
        
        graphicsDropdown.value = _qualityLevel;
    }

    void Update()
    {
        // every time the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("PauseMenuCanvas").SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    // method is called when the slider value is changed
    // and it updates the text value accordingly
    public void SetVolume(float volume)
    {
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("New quality index: " + qualityIndex);
        _qualityLevel= qualityIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
//        Resolution resolution= resolutions[resolutionIndex];
//        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ApplySettings()
    {
    // VOLUME
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        AudioListener.volume = volumeSlider.value;
        volumeBeforeChange= AudioListener.volume;
        Debug.Log("Settings applied");

    // GRAPHICS
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

    // RESOLUTION
        PlayerPrefs.SetFloat("masterResolutionHeight", resolutions[resolutionDropdown.value].height);
        PlayerPrefs.SetFloat("masterResolutionWidth", resolutions[resolutionDropdown.value].width);

        Resolution resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }

    // this method is called when BACK is pressed
    public void CancelChanges()
    {
    // VOLUME
        volumeTextValue.text = volumeBeforeChange.ToString("0.0");
        volumeSlider.value = volumeBeforeChange;

    // GRAPHICS
        _qualityLevel = QualitySettings.GetQualityLevel();
        graphicsDropdown.value = _qualityLevel;

    // RESOLUTION
        string currentResolution = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
        _resolutionIndex = Array.IndexOf(resolutions, currentResolution);
        resolutionDropdown.value = _resolutionIndex;
    }

    public void RestoreSettings()
    {
    // VOLUME
        volumeTextValue.text = defaultVolume.ToString("0.0");
        volumeSlider.value = defaultVolume;
        volumeBeforeChange = defaultVolume;

    // GRAPHICS
        _qualityLevel = defaultQualityIndex;
        graphicsDropdown.value = _qualityLevel;

    // RESOLUTION
        _resolutionIndex = defaultResolutionIndex;
        resolutionDropdown.value = _resolutionIndex;
    }
}

// https://www.youtube.com/watch?v=Cq_Nnw_LwnI
