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
    private Channel previousChannelSource;
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
        foreach(Channel channel in radioChannels)
        {
            if (channel.GetChannelVolume() > 0f)
            {
                return true;
            }
        }
        return false;
    }
    public void LowerSound(bool isNext)
    {
        
    }
    
    public void Channel(bool isNext = true)
    {

        if (isNext||currentChannelIndex==-1)
        {
            if(currentChannelIndex >= radioChannels.Length-1)
            {
                CloseRadio();
                currentChannelIndex = (currentChannelIndex + 1) % radioChannels.Length;
                return;
            }
            currentChannelIndex = (currentChannelIndex + 1) % radioChannels.Length;
        }
        SoundManager.instance.PlaySound(RadioOn);
        
        radioChannels[currentChannelIndex].On();
        if (previousChannelSource != null)
        {
            
            previousChannelSource.Off();
        }
        previousChannelSource = radioChannels[currentChannelIndex];
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
        previousChannelSource.Off();
        previousChannelSource = null;
    }
    private void OpenRadio()
    {
        Channel(false);
        BGMusic.volume = 0f;
    }
    
}
