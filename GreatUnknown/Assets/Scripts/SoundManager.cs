using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } //to get music in other group
    private AudioSource source;
    
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip _sound, AudioSource otherSource = null)
    {
        if(otherSource != null)
        {
            otherSource.PlayOneShot(_sound);
        }
        else
        {
            source.PlayOneShot(_sound);
        }
    }
    public void PlaySound(AudioInfo _audioInfo, bool stopPrevious = false, AudioSource otherSource = null)
    {
        if(otherSource != null)
        {
            if(stopPrevious)
            {
                otherSource.Stop();
            }
            Debug.Log("pLAY sound");
            otherSource.clip = _audioInfo.soundClip;
            otherSource.Play();
            otherSource.volume = _audioInfo.volume;
            otherSource.loop = _audioInfo.isLooping;
        }
        else
        {
            if(stopPrevious)
            {
                source.Stop();
            }
            source.PlayOneShot(_audioInfo.soundClip, _audioInfo.volume);
            source.loop = _audioInfo.isLooping;
        }
    }
}
