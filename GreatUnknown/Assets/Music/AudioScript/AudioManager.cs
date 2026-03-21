using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager: MonoBehaviour
{
    public static AudioManager instance { get; private set; } //to get music in other group
    [SerializeField] private AudioMixer mixer;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void SetMusicVolume(Volume volumeType, float value)
    {
        mixer.SetFloat(volumeType.ToString(), Mathf.Log10(value) * 20);
    }
}