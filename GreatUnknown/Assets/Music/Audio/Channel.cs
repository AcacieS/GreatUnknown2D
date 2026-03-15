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
    public bool IsFinish()
    {
        return time>radioChannelSource.clip.length - 0.01f;
    }
    public AudioInfo GetChannelInfo()
    {
        return radioChannel;
    }
    public void SwitchAudio(AudioInfo newRadioChannel = null)
    {
        if(newRadioChannel == null)
        {
            newRadioChannel = radioChannel;
        }
        radioChannelSource.clip = newRadioChannel.soundClip;
        radioChannelSource.loop = newRadioChannel.isLooping;
        radioChannel = newRadioChannel;
    }

    public void SetTimeMusic(float newTime = 0f)
    {
        radioChannelSource.time = Mathf.Clamp(newTime, 0f, radioChannelSource.clip.length - 0.01f);
    }
    public void Off()
    {
        SetVolume(0f);
    }
    public void On()
    {
        SetVolume(radioChannel.volume);
    }
    private void SetVolume(float volume = 0f)
    {
        radioChannelSource.volume = volume;
    }
    public AudioSource GetChannelAudioSource()
    {
        return radioChannelSource;
    }
    public float GetChannelVolume()
    {
        return radioChannelSource.volume;
    }
    
    public void PlayChannel(AudioInfo newStoryInfo)
    {
        radioChannel = newStoryInfo;
        radioChannelSource.clip = radioChannel.soundClip;
        radioChannelSource.volume = radioChannel.volume;
        radioChannelSource.loop = radioChannel.isLooping;
        radioChannelSource.Play();
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