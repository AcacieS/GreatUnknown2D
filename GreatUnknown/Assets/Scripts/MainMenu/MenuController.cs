using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    [Header ("General Setting")]
    [SerializeField] private bool useSavedValues = false; // should we load prefs or not
    [Header ("Overall Volume settings")]
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [Header ("Music settings")]
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private TMP_Text musicTextValue = null;
    [Header ("SFX settings")]
    [SerializeField] private Slider SFXslider = null;
    [SerializeField] private TMP_Text SFXTextValue = null;

    /*
    [Header ("GamePlay Settings")]
    [SerializeField] private TMP_Text SensTextValue = null;
    [SerializeField] private Slider SensSlide = null;
    [SerializeField] private int defaultSens = 4;
    public int mainControllerSens = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;
    */
    [Space(10)]

    [Header("Quality Settings")]
    // [SerializeField] private Slider brightnessSlider = null;
    // [SerializeField] private TMP_Text brightTextValue = null;
    // [SerializeField] private float defaultBrightness = 1;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [Header("FullScreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;
    [Space(10)]
    private int qualityLevel;
    private bool isFullScreen;
    //private float brightnessLevel;
    /*
    [Header("Confirmation")]
    [SerializeField] private GameObject confirmPrompt = null;
    */

    [Header ("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Awake()
    {
        if (useSavedValues)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeSlider.value = localVolume;
                
                float localMusic = PlayerPrefs.GetFloat("masterMusic");
                musicSlider.value = localMusic;

                float localSFX = PlayerPrefs.GetFloat("masterSFX");
                SFXslider.value = localSFX;

                SetVolume();
                SetMusicVolume();
                SetSFXVolume();

                volumeTextValue.text = localVolume.ToString("0.0");
                musicTextValue.text = localMusic.ToString("0.0");
                SFXTextValue.text = localSFX.ToString("0.0");
            }
            else
            {
                ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            if (PlayerPrefs.HasKey("masterFullScreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");
                if (localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                } else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }
            /*
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessSlider.value = localBrightness; 
                // change brightness
            }
            */

        }
    }

    void Start()
    {
        SetSFXVolume();
        SetMusicVolume();
        SetVolume();
        
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        volumeTextValue.text = volume.ToString("0.0");
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        mixer.SetFloat("MenuMusicVolume",Mathf.Log10(volume) * 20);
        musicTextValue.text = volume.ToString("0.0");
    }

    public void SetSFXVolume()
    {
        float volume = SFXslider.value;
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        mixer.SetFloat("SubmarineVolume", Mathf.Log10(volume) * 20);
        mixer.SetFloat("DayTransitionVolume", Mathf.Log10(volume) * 20);
        SFXTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        // Save value of Volume in variable masterVolume
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("masterMusic", musicSlider.value);
        PlayerPrefs.SetFloat("masterSFX", SFXslider.value);
        // StartCoroutine(ConfirmationBox());
    }

    /*
    public void SetControllerSens(float sensitivity) // we get float
    {
        mainControllerSens = Mathf.RoundToInt(sensitivity); // but we need whole int
        SensTextValue.text = sensitivity.ToString("0"); 
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            // value is 1 true or 0 false
            PlayerPrefs.SetInt("masterInvertyY",1);
        } else
        {
            PlayerPrefs.SetInt("masterInvertY",0);
        } 
        PlayerPrefs.SetFloat("masterSens", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightTextValue.text = brightness.ToString("0.0");
    }
    */
    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
    }
    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }
    public void GraphicsApply()
    {
        //PlayerPrefs.SetFloat("masterBrightness",brightnessLevel);
        // change your brightness with ur post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;

        // StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(String MenuType)
    {
        if (MenuType == "Audio")
        {
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");

            musicSlider.value = defaultVolume;
            musicTextValue.text = defaultVolume.ToString("0.0");

            SFXslider.value = defaultVolume;
            SFXTextValue.text = defaultVolume.ToString("0.0");
            SetVolume();
            SetMusicVolume();
            SetSFXVolume();
            VolumeApply(); //save value
        }
        /*
        if (MenuType == "Gameplay")
        {
            SensTextValue.text = defaultSens.ToString("0");
            SensSlide.value = defaultSens;
            mainControllerSens = defaultSens;
            invertYToggle.isOn = false;
            GameplayApply();
        }
        */
        if (MenuType == "Graphics")
        {
            // Reset brightness value
            // brightnessSlider.value = defaultBrightness;
            // brightTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height,Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length; // last is max, like the screen
            GraphicsApply();
        }
    }

    /*

    public IEnumerator ConfirmationBox()
    {
        confirmPrompt.SetActive(true);
        // Pauses execution for a specified amount of time. The coroutine resumes after the specified number of seconds has elapsed.
        yield return new WaitForSeconds(2);
        confirmPrompt.SetActive(false);
    }
    */
}
