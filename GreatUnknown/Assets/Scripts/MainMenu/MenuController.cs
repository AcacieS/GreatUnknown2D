using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;
using UnityEditor.Scripting;
using UnityEngine.Rendering;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using Unity.VisualScripting;
using System.Collections.Generic;
using NUnit.Framework;

public class MenuController : MonoBehaviour
{
    [Header ("Volume settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header ("GamePlay Settings")]
    [SerializeField] private TMP_Text SensTextValue = null;
    [SerializeField] private Slider SensSlide = null;
    [SerializeField] private int defaultSens = 4;
    public int mainControllerSens = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;


    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;


    [Header("Confirmation")]
    [SerializeField] private GameObject confirmPrompt = null;
    
    [Header ("Levels to load")]
    public string newGameScene;
    private string dayToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header ("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Start()
    {
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

    public void NewGameYes()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void LoadGameYes()
    {
        if (PlayerPrefs.HasKey("SavedDay"))
        {
            dayToLoad = PlayerPrefs.GetString("SavedDay");
            SceneManager.LoadScene(dayToLoad);
        } else
        {
            noSavedGameDialog.SetActive(true);
        }
    
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        // Save value of Volume in variable masterVolume
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

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
        PlayerPrefs.SetFloat("masterBrightness",brightnessLevel);
        // change your brightness with ur post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(String MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply(); //save value
        }
        if (MenuType == "Gameplay")
        {
            SensTextValue.text = defaultSens.ToString("0");
            SensSlide.value = defaultSens;
            mainControllerSens = defaultSens;
            invertYToggle.isOn = false;
            GameplayApply();
        }
        if (MenuType == "Graphics")
        {
            // Reset brightness value
            brightnessSlider.value = defaultBrightness;
            brightTextValue.text = defaultBrightness.ToString("0.0");

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

    public IEnumerator ConfirmationBox()
    {
        confirmPrompt.SetActive(true);
        // Pauses execution for a specified amount of time. The coroutine resumes after the specified number of seconds has elapsed.
        yield return new WaitForSeconds(2);
        confirmPrompt.SetActive(false);
    }
}
