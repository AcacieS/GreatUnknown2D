using System.Collections;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } //to get music in other group
    [SerializeField] private SoundCatalog soundCatalog;
    private AudioSource source;
    
    private void Awake()
    {
        instance = this;
        if (soundCatalog == null) Ext.WarnRefAndDisable("soundCatalog", this);
        source = GetComponent<AudioSource>();
    }

    public AudioInfo GetSoundCatalogue(string soundName)
    {
        if (!soundCatalog.TryGet(soundName, out var audioInfo) || audioInfo==null)
        {
            Debug.LogError($"[Sound Machine] Unknown sound id '{soundName}'. Add it to Sound Catalog.");
            return null;
        }
        return audioInfo;
    }
    public void PlaySound(string soundName)
    {
        if (!soundCatalog.TryGet(soundName, out var audioInfo) || audioInfo==null)
        {
            Debug.LogError($"[Sound Machine] Unknown sound id '{soundName}'. Add it to Sound Catalog.");
            return;
        }

        PlaySound(audioInfo);
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
    public void PlaySound(AudioInfo _audioInfo, SoundState changePreviousState = SoundState.None, PlaySoundState playSoundState = PlaySoundState.PlayOneShot, AudioSource otherSource = null)
    {
        if(otherSource == null)
        {
            otherSource = source;
        }

        if(changePreviousState == SoundState.FadeOut)
        {
            FadeOut(1f, _audioInfo, playSoundState, otherSource);
        }
        else
        {
            if(changePreviousState == SoundState.Cut)
            {
                otherSource.Stop(); //might not the right source
            }

            if(playSoundState == PlaySoundState.Play)
            {
                PlayNormalSound(_audioInfo, otherSource);
            }
            else
            {
                PlayOneShotSound(_audioInfo, otherSource);
            }
            
        }

    }
    private void PlayNormalSound(AudioInfo _audioInfo, AudioSource otherSource)
    {
        otherSource.clip = _audioInfo.soundClip;
        otherSource.Play();
        otherSource.volume = _audioInfo.volume;
        otherSource.loop = _audioInfo.isLooping;
    }

    private void PlayOneShotSound(AudioInfo _audioInfo, AudioSource otherSource)
    {
        if (otherSource == null)
        {
            Debug.LogError("Attempt to play on a null audio source");
            return;
        }
        otherSource.PlayOneShot(_audioInfo.soundClip, _audioInfo.volume);
        otherSource.loop = _audioInfo.isLooping;
    }

    public void FadeOut(float duration, AudioInfo _audioInfo, PlaySoundState playSoundState, AudioSource otherSource = null)
    {
        if (otherSource == null)
        {
            otherSource = source;
        }
        StartCoroutine(FadeOutCoroutine(duration, _audioInfo, playSoundState,otherSource));
    }

    private IEnumerator FadeOutCoroutine(float duration, AudioInfo _audioInfo, PlaySoundState playSoundState,AudioSource sourceToFade)
    {
        float startVolume = sourceToFade.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            sourceToFade.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        sourceToFade.volume = 0f;
        sourceToFade.Stop();
        if(playSoundState == PlaySoundState.Play)
        {
            PlayNormalSound(_audioInfo, sourceToFade);
        }
        else
        {
            PlayOneShotSound(_audioInfo, sourceToFade);
        }
        sourceToFade.volume = 1f;
    }
}
