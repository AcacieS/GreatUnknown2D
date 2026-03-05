using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour, IClickable
{
    private AudioSource source;
    [SerializeField] private AudioSource BGMusic;
    [SerializeField] private AudioInfo RadioOn;
    [SerializeField] private AudioInfo RadioOff;
    [SerializeField] private AudioSource[] casualChannelsSource;
    [SerializeField] private Channel[] radioChannels; 
    private int currentChannelIndex = -1;
    void Start()
    {
        source = GetComponent<AudioSource>();
        // source.volume = 0f;
    }
    public void OnClick()
    {
        // Debug.Log("Channel changed");
        // Channel();
        if(RadioIsOn()) //so is on
        {
            //CloseRadio();
            
            Channel();
        }
        else //so is off
        {
            OpenRadio();
        }
    }
    public bool RadioIsOn()
    {
        foreach(AudioSource channelSource in casualChannelsSource)
        {
            if (channelSource.volume > 0f)
            {
                return true;
            }
        }
        return false;
    }
    public void LowerSound(bool isNext)
    {
        
    }
    private Channel previousChannel;
    AudioSource previousChannelSource;
    public void Channel(bool isNext = true)
    {

        if (isNext||currentChannelIndex==-1)
        {
            if(currentChannelIndex >= casualChannelsSource.Length-1)
            {
                CloseRadio();
                currentChannelIndex = (currentChannelIndex + 1) % casualChannelsSource.Length;
                return;
            }
            currentChannelIndex = (currentChannelIndex + 1) % casualChannelsSource.Length;
        }
        SoundManager.instance.PlaySound(RadioOn);
        casualChannelsSource[currentChannelIndex].volume = 1f;
        if (previousChannelSource != null)
        {
            previousChannelSource.volume = 0f;
        }
        previousChannelSource = casualChannelsSource[currentChannelIndex];
    }
    public void OpenCloseRadio()
    {
        Debug.Log("Toggle radio");
        if(RadioIsOn()) //so is on
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
        //source.volume = 0f;
        BGMusic.volume = 0.5f;
        previousChannelSource.volume= 0f;
        previousChannelSource = null;
    }
    private void OpenRadio()
    {
        Channel(false);
        BGMusic.volume = 0f;
    }
    
}
