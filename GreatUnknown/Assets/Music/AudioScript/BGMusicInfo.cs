using UnityEngine;

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
        audioSourceGO.SetActive(true);
        audioSource.GetComponent<AudioSource>().volume = audioInfo.volume;
        audioSource.GetComponent<AudioSource>().loop = audioInfo.isLooping;
    }
    public void Desactivate(AudioInfo audioInfo)
    {
        audioSourceGO.SetActive(false);
    }
}