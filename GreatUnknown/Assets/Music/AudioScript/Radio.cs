using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour, IClickable
{
    public static Radio Instance {get; private set;}
    [Header("Radio SFX")]
    [SerializeField] private AudioInfo RadioOn;
    [SerializeField] private AudioInfo RadioOff;
    [Header("Channel")]
    [SerializeField] private List<Channel> radioChannels; 
    [Header("Story Radio")]
    [SerializeField] private float shootingCountDown = 30f;
    [SerializeField] private Channel environmentalChannel;
    [SerializeField] private Channel shootingChannel;
    private int currentChannelIndex = -1;
    private Channel previousChannelSource;
    //----- Story variable 
    private bool isShootingStory = false;
    private Channel channel3 = null;
    private float channel3TimeDay3 = 0;
    private bool firstTimeEnvironmentalStory = true;

    void Start()
    {
    }
    public void Save()
    {
        // foreach(Channel radioChannel in radioChannels)
        // {
        //     radioChannel.SaveTime();
        // }
    }
    public void Reset()
    {
        // if(GameManagement.Instance.GetNbDayPassed()==2 && channel3!=null && radioChannels[2] == environmentalChannel)
        // {
        //     radioChannels[2] = channel3;
        // }

        if (GameManagement.Instance.GetNbDayPassed() == 3) //day 4
        {
            firstTimeEnvironmentalStory = true;
        }

        if (GameManagement.Instance.GetNbDayPassed() == 4) //day 5
        {
            StopAllCoroutines();
            isShootingStory = false;
        }
        foreach(Channel radioChannel in radioChannels)
        {
            //radioChannel.SetTimeSavedMusic();
        }
    }
    public void OnClick()
    {
        if (GameManagement.Instance.GetNbDayLeft()==0)
        {
            SoundManager.instance.PlaySound(RadioOff);
            return;
        }
        if (OpenCloseRadioStory()) {
            Debug.Log("Hey shooting");
            return;
        }
        if(RadioIsOn()) //so is on
        {
            Debug.Log("Hey change radio");
            //change Channel
            Channel();
        }
        else //so is off
        {
            Debug.Log("Hey open radio");
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
    
    public void Channel(bool isNext = true)
    {
        if (isNext||currentChannelIndex==-1)
        {
            if(currentChannelIndex >= radioChannels.Count-1)
            {
                CloseRadio();
                currentChannelIndex = (currentChannelIndex + 1) % radioChannels.Count;
                return;
            }
            currentChannelIndex = (currentChannelIndex + 1) % radioChannels.Count;
        }
        
        SoundManager.instance.PlaySound(RadioOn);

        if (GameManagement.Instance.GetNbDayPassed() == 3 && firstTimeEnvironmentalStory)
        {
            environmentalChannel.SetTimeMusic();
            firstTimeEnvironmentalStory = false;
        }
        Debug.Log("radioChannels should play");
        radioChannels[currentChannelIndex].On();
        if (previousChannelSource != null)
        {
            previousChannelSource.Off();
        }
        previousChannelSource = radioChannels[currentChannelIndex];
    }
    public void CloseAllChannelRadio()
    {
        if (previousChannelSource != null)
        {
            previousChannelSource.Off();
        }
    }

    private void CloseRadio()
    {
        SoundManager.instance.PlaySound(RadioOff);
        //source.volume = 0f;
        //BGMusic.volume = 0.5f;
        previousChannelSource.Off();
        previousChannelSource = null;
    }
    private void OpenRadio()
    {
        Channel(false);
        //BGMusic.volume = 0f;
    }
    
    //-------------------------------- STORY ----------------------
    public void ChangeRadioChannel()
    {
        if (channel3 == null)
        {
            if(previousChannelSource == radioChannels[2])
            {
                previousChannelSource.Off();
            }
            channel3 = radioChannels[2];
            channel3TimeDay3 = channel3.GetTime();
        }
        
        radioChannels[2] = environmentalChannel;
        environmentalChannel.SetTimeMusic();

        if (previousChannelSource == channel3)
        {
            environmentalChannel.On();
            firstTimeEnvironmentalStory = false;
            previousChannelSource = environmentalChannel;
        }
    }
    
    public void ShootingRadioChannel()
    {
        if (channel3 == null) //for skipping day
        {
            channel3 = radioChannels[2];
            channel3TimeDay3 = channel3.GetTime();
        }

        channel3.SetTimeMusic(channel3TimeDay3);
        radioChannels[2] = channel3;

        if(previousChannelSource == environmentalChannel) { //playing radio 3
            environmentalChannel.Off();
            radioChannels[2].On();
            previousChannelSource = radioChannels[2];
        }
        
        GameManagement.Instance.CallStartCountDownShooting(shootingCountDown);
    }
    
    
    private bool OpenCloseRadioStory()
    {
        if (isShootingStory)
        {
            if(shootingChannel.GetChannelVolume() == 0.0f) //so is off
            {
                SoundManager.instance.PlaySound(RadioOn);
                Debug.Log("On");
                shootingChannel.On();
            }
            else //
            {
                Debug.Log("Off: "+shootingChannel.GetChannelVolume());
                SoundManager.instance.PlaySound(RadioOff);
                shootingChannel.Off();
            }
            return true;
        }
        return false;
    }
    //----------------------------------- BUTTON RADIO --------------------
    public void OpenCloseRadio()
    {
        if(OpenCloseRadioStory()) return;
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
    public void PlayShooting()
    {
        Debug.Log("Should be playing shooting");
        isShootingStory = true;
        if (previousChannelSource != null)
        {
            previousChannelSource.Off();
            previousChannelSource = shootingChannel;
        }
        
        shootingChannel.SetTimeMusic();
        shootingChannel.PlayRadio();
    }
        
    
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
