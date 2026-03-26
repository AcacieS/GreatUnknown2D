using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System;
using NUnit.Framework;
using System.Linq.Expressions;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private BookViewerUI bookViewerUI;
    [SerializeField] private FaxViewerUI faxViewerUI;
    [SerializeField] private GameObject pauseMenuUI = null;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource pauseMenuMusic;

    public static PauseMenu instance {get; private set; }
    public static bool isPaused = false;
    public string mainMenuScene;

    private float savedSFX, savedSubmarine, savedPrintNoise, savedMusic, savedRadio, savedDayTransition; 

    private Boolean isSubMenuOpen = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject); // Delete duplicates if we return to the start scene
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnChangeScene;

        if (bookViewerUI == null) { Ext.WarnRef("bookViewerUI", this); return; }
        if (faxViewerUI == null) { Ext.WarnRef("faxViewerUI", this); return; }
    }

    void OnChangeScene(Scene previous, Scene next)
    {
        if (next.name == mainMenuScene) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        bool canPause = true;
        if (bookViewerUI != null && faxViewerUI != null)
        {
            canPause = !bookViewerUI.gameObject.activeSelf && !faxViewerUI.transform.parent.gameObject.activeSelf;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused && !isSubMenuOpen) // ignore escape if submenu is open
                Resume();
            else if (canPause)
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuMusic.Stop();
        RestoreVolume();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuMusic.Play();
        MuteVolume();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void SubMenuOpen()
    {
        isSubMenuOpen = true;
    }

    public void SubMenuClose()
    {
        isSubMenuOpen = false;
    }


    void MuteVolume()
    {
        mixer.GetFloat("SFXVolume", out savedSFX);
        mixer.GetFloat("DayTransitionVolume", out savedDayTransition);
        mixer.GetFloat("MusicVolume", out savedMusic);
        mixer.GetFloat("PrintNoiseVolume", out savedPrintNoise);
        mixer.GetFloat("RadioVolume", out savedRadio);
        mixer.GetFloat("SubmarineVolume", out savedSubmarine);

        float muted = Mathf.Log10(0.0001f) * 20; // -80db

        mixer.SetFloat("SFXVolume", muted);
        mixer.SetFloat("DayTransitionVolume", muted);
        mixer.SetFloat("MusicVolume", muted);
        mixer.SetFloat("PrintNoiseVolume", muted);
        mixer.SetFloat("RadioVolume", muted);
        mixer.SetFloat("SubmarineVolume", muted);
    }
    void RestoreVolume()
    {
        mixer.SetFloat("SFXVolume", savedSFX);
        mixer.SetFloat("DayTransitionVolume", savedDayTransition);
        mixer.SetFloat("MusicVolume", savedMusic);
        mixer.SetFloat("PrintNoiseVolume", savedPrintNoise);
        mixer.SetFloat("RadioVolume", savedRadio);
        mixer.SetFloat("SubmarineVolume", savedSubmarine);
    }
}
