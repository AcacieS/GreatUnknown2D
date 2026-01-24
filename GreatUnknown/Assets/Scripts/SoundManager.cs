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
    public void PlaySound(AudioClip _sound, bool is_animalSound = false)
    {
        source.PlayOneShot(_sound);
    }
}
