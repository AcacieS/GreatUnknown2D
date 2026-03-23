using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LastDay : MonoBehaviour
{
    [SerializeField] private int klaxonDelaySeconds = 3;
    [SerializeField] private int promptDelaySeconds = 5;
    [SerializeField] private AudioInfo Klaxon;
    [SerializeField] private AudioInfo LastDayAmbiance;
    [SerializeField] private AudioSource ambianceSource;
    [SerializeField] private AudioSource klaxonSource;
    [SerializeField] private GameObject LastDayTerminal;
    [SerializeField] private Animator EmergencyLightsAnimator;
    [SerializeField] private Animator ComputerAnimator;
    [SerializeField] private ClickHandler ComputerClickHandler;
    [SerializeField] private string natureEndingScene;
    [SerializeField] private string industrialEndingScene;
    [SerializeField] private UnityEvent LastDayEnabled;

    // This script lies dormant until the last day.
    void OnEnable()
    {
        PlaySound(LastDayAmbiance, ambianceSource);
        StartCoroutine(DelayKlaxonSound());
        LastDayEnabled?.Invoke();
    }


    private void PlaySound(AudioInfo audioInfo, AudioSource audioSource)
    {
        audioSource.volume = audioInfo.volume;
        audioSource.clip = audioInfo.soundClip;
        audioSource.loop = audioInfo.isLooping;
        audioSource.Play();
    }

    private IEnumerator DelayKlaxonSound()
    {
        yield return new WaitForSeconds(klaxonDelaySeconds);

        Debug.Log("Playing the Klaxon Sound");
        PlaySound(Klaxon, klaxonSource);
        EmergencyLightsAnimator.SetBool("On", true);

        yield return new WaitForSeconds(promptDelaySeconds);

        ComputerAnimator.SetTrigger("Flash");
        ComputerClickHandler.onClick.RemoveAllListeners();
        ComputerClickHandler.onClick.AddListener(FinalTerminalScene);
    }

    public void FinalTerminalScene()
    {
        LastDayTerminal.SetActive(true);
        EmergencyLightsAnimator.SetBool("On", false);
    }

    public void NatureEnding()
    {
        SceneManager.LoadSceneAsync(natureEndingScene);
    }

    public void IndustrialEnding()
    {
        SceneManager.LoadSceneAsync(industrialEndingScene);
    }
}
