using System;
using UnityEngine;

[Serializable]
public class BGMusicInfo
{
    public GameObject audioSourceGO;
    private AudioSource audioSource;
    public void AddAudioSource()
    {
        this.audioSource = audioSourceGO.GetComponent<AudioSource>();
    }
    
    public void Activate(AudioInfo audioInfo)
    {
        SoundManager.instance.PlaySound(audioInfo, SoundState.None, PlaySoundState.Play, audioSource);
        // audioSourceGO.SetActive(true);
        // audioSource.clip = audioInfo.soundClip;
        // audioSource.GetComponent<AudioSource>().volume = audioInfo.volume;
        // audioSource.GetComponent<AudioSource>().loop = audioInfo.isLooping;
    }
    public void Desactivate(AudioInfo audioInfo)
    {
        audioSourceGO.SetActive(false);
    }
}