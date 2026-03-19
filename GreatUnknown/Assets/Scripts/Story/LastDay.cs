using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastDay : MonoBehaviour
{
    [SerializeField] private int klaxonDelaySeconds = 3;
    [SerializeField] private int promptDelaySeconds = 5;
    [SerializeField] private AudioInfo Klaxon;
    [SerializeField] private AudioInfo LastDayAmbiance;
    [SerializeField] private AudioSource ambianceSource;
    [SerializeField] private AudioSource klaxonSource;
    [SerializeField] private GameObject LastDayTerminal;
    [SerializeField] private GameObject LastDayPrompt;
    [SerializeField] private Animator EmergencyLightsAnimator;

    // This script lies dormant until the last day.
    void OnEnable()
    {
        PlaySound(LastDayAmbiance, ambianceSource);
        StartCoroutine(DelayKlaxonSound());
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

        LastDayPrompt.SetActive(true);
    }

    public void FinalTerminalScene()
    {
        LastDayPrompt.SetActive(false);
        LastDayTerminal.SetActive(true);
        EmergencyLightsAnimator.SetBool("On", false);
    }
}
