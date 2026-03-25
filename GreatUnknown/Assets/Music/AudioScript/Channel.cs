using System;
using UnityEngine;

[Serializable]
public class Channel
{
    [SerializeField] public AudioSource radioChannelSource;
    [SerializeField] private AudioInfo radioChannel; 
    [SerializeField] private float time;
    
    public void SaveTime()
    {
        if (radioChannelSource.clip != null)
        time = radioChannelSource.time;
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
    public float GetTime()
    {
        return time;
    }
    public void SetTimeSavedMusic()
    {
        radioChannelSource.time = Mathf.Clamp(time, 0f, radioChannelSource.clip.length - 0.01f);
    }
    public void SetTimeMusic(float newTime = 0f)
    {
        Debug.Log("time is set to that value");
        radioChannelSource.time = Mathf.Clamp(newTime, 0f, radioChannelSource.clip.length - 0.01f);
    }
    public void Off()
    {
        SetVolume(0f);
    }
    public void On()
    {
        Debug.Log("volume of radioChannel  "+radioChannel.volume+"and time: "+time);
        SetVolume(radioChannel.volume);
        if (!radioChannelSource.isPlaying)
        {
            radioChannelSource.Play();
        }
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
            previousChannel.SaveTime();
        }
        Debug.Log("pLAY CHANNEL");
        radioChannelSource.time = Mathf.Clamp(time, 0f, radioChannelSource.clip.length - 0.01f);
        SoundManager.instance.PlaySound(radioChannel, SoundState.None, PlaySoundState.Play, radioChannelSource);
    }
}