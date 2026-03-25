using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BGMusicInfo", menuName = "Scriptable Objects/BGMusicInfo")]
public class BGMusicInfo: ScriptableObject
{
    public int dayStart;
    public int dayEnd;
    public string audioName;
    public GameObject audioSourceGO;
    private AudioSource audioSource;
    public void AddAudioSourceGO(GameObject pAudioSourceGO)
    {
        audioSourceGO = pAudioSourceGO;
        AddAudioSource();
    }
    public void AddAudioSource()
    {
        this.audioSource = audioSourceGO.GetComponent<AudioSource>();
    }

    public void Activate(AudioInfo audioInfo)
    {
        if(audioSourceGO.activeSelf) return;
        if (GameManagement.Instance.GetNbDayPassed()+1 >= dayStart)
        {
            audioSourceGO.SetActive(true);
            audioSource.clip = audioInfo.soundClip;
            audioSource.GetComponent<AudioSource>().volume = audioInfo.volume;
            audioSource.GetComponent<AudioSource>().loop = audioInfo.isLooping;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        
    }
    public void Desactivate()
    {
        if(!audioSourceGO.activeSelf) return;
        if (GameManagement.Instance.GetNbDayPassed()+1 > dayEnd)
        {
            audioSourceGO.SetActive(false);
        }
        
    }
}