using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Localization.Platform.iOS;
using UnityEngine.SocialPlatforms;

public class LoadPrefs : MonoBehaviour
{
    [Header ("General Setting")]
    [SerializeField] private bool canUse = false; // should we load prefs or not
    [SerializeField] private MenuController menuController; //script

    [Header ("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [Header ("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightTextValue = null;
    [Header ("Quality level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [Header ("FullScreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    // before ANY method 
    void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
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
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessSlider.value = localBrightness; 
                // change brightness
            }

        }
    }
}
