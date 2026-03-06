using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header ("Volume settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmPrompt = null;
    [SerializeField] private float defaultVolume = 1.0f;
    
    
    [Header ("Levels to load")]
    public string newGameScene;
    private string dayToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

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

    public void ResetButton(String MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply(); //save value
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
