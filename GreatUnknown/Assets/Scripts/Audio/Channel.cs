using System;
using UnityEngine;

[Serializable]
public class Channel
{
    [SerializeField] public AudioSource radioChannelSource;
    [SerializeField] private AudioInfo radioChannel; 
    [SerializeField] private float time;
    
    private void SaveTime(AudioSource source)
    {
        if (source.clip != null)
        time = source.time;
    }
    public AudioInfo GetChannelInfo()
    {
        return radioChannel;
    }
    public void Off()
    {
        SetVolume(0f);
    }
    public void On()
    {
        SetVolume(radioChannel.volume);
    }
    private void SetVolume(float volume)
    {
        radioChannelSource.volume = volume;
    }
    public AudioSource GetChannelAudioSource()
    {
        return radioChannelSource;
    }
    public float GetChannelVolume()
    {
        return radioChannel.volume;
    }
    public void PlayChannelWithTime(Channel previousChannel)
    {
        if (previousChannel!=null)
        {
            previousChannel.SaveTime(radioChannelSource);
        }
        Debug.Log("pLAY CHANNEL");
        SoundManager.instance.PlaySound(radioChannel, SoundState.None, PlaySoundState.Play, radioChannelSource);
        radioChannelSource.time = Mathf.Clamp(time, 0f, radioChannelSource.clip.length - 0.01f);
    }
}