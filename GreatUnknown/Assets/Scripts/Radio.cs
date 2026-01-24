using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour, IClickable
{
    private AudioSource source;
    [SerializeField] private AudioSource BGMusic;
    [SerializeField] private AudioClip RadioOn;
    [SerializeField] private AudioClip RadioOff;
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0f;
    }
    public void OnClick()
    {
        if(source.volume > 0f) //so is on
        {
            SoundManager.instance.PlaySound(RadioOff);
            source.volume = 0f;
            BGMusic.volume = 0.5f;
        }
        else //so is off
        {
            SoundManager.instance.PlaySound(RadioOn);
            source.volume = 0.2f;
            BGMusic.volume = 0f;
        }
    }
    
}
