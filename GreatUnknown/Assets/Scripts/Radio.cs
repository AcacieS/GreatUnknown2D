using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour, IClickable
{
    private AudioSource source;
    [SerializeField] private AudioSource BGMusic;
    [SerializeField] private AudioInfo RadioOn;
    [SerializeField] private AudioInfo RadioOff;
    [SerializeField] private Channel[] radioChannels; 
    private int currentChannelIndex = -1;
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0f;
    }
    public void OnClick()
    {
        // Debug.Log("Channel changed");
        // Channel();
        if(source.volume > 0f) //so is on
        {
            //CloseRadio();
            
            Channel();
        }
        else //so is off
        {
            OpenRadio();
        }
    }
    private Channel previousChannel;
    public void Channel(bool isNext = true)
    {
        if (isNext||currentChannelIndex==-1)
        {
            currentChannelIndex = (currentChannelIndex + 1) % radioChannels.Length;
        }
        SoundManager.instance.PlaySound(RadioOn);
        radioChannels[currentChannelIndex].PlayChannel(source, previousChannel);
        previousChannel = radioChannels[currentChannelIndex];
    }
    public void OpenCloseRadio()
    {
        Debug.Log("Toggle radio");
        if(source.volume > 0f) //so is on
        {
            CloseRadio();
        }
        else //so is off
        {
            OpenRadio();
        }
    }
    private void CloseRadio()
    {
        SoundManager.instance.PlaySound(RadioOff);
        source.volume = 0f;
        BGMusic.volume = 0.5f;
    }
    private void OpenRadio()
    {
        Channel(false);
        BGMusic.volume = 0f;
    }
    
}
