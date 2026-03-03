using System;
using UnityEngine;

[Serializable]
public class Channel
{
    [SerializeField] private AudioInfo radioChannel; 
    [SerializeField] private float time;
    public void SaveTime(AudioSource source)
    {
        if (source.clip != null)
        time = source.time;
    }
    public void PlayChannel(AudioSource source, Channel previousChannel)
    {
        if (previousChannel!=null)
        {
            previousChannel.SaveTime(source);
        }
        Debug.Log("pLAY CHANNEL");
        SoundManager.instance.PlaySound(radioChannel, false, source);
        source.time = Mathf.Clamp(time, 0f, source.clip.length - 0.01f);
    }
}