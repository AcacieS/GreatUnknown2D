using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour, IClickable
{
    public static Radio Instance {get; private set;}
    private AudioSource source;
    [Header("Radio SFX")]
    [SerializeField] private AudioInfo RadioOn;
    [SerializeField] private AudioInfo RadioOff;
    [SerializeField] private AudioSource BGMusic;
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
    private bool firstTimeEnvironmentalStory = true;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void OnClick()
    {
        if (OpenCloseRadioStory()) {
            
            return;
        }
        if(RadioIsOn()) //so is on
        {
            //change Channel
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
            environmentalChannel.SetTimeMusic(0f);
            firstTimeEnvironmentalStory = false;
        }

        radioChannels[currentChannelIndex].On();
        if (previousChannelSource != null)
        {
            previousChannelSource.Off();
        }
        previousChannelSource = radioChannels[currentChannelIndex];
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
    public void ResetRadioDay()
    {
        
    }
    public void SaveRadios()
    {
        
    }
    
    //-------------------------------- STORY ----------------------
    public void ChangeRadioChannel()
    {
        if(previousChannelSource == radioChannels[2])
        {
            previousChannelSource.Off();
        }

        channel3 = radioChannels[2];
        radioChannels[2] = environmentalChannel;
        Debug.Log("Hey channel 3 is removed");
    }
    
    public void ShootingRadioChannel()
    {
        radioChannels[2] = channel3;
        StopAllCoroutines();
        StartCoroutine(StartCountDownShooting());
    }
    
    
    private IEnumerator StartCountDownShooting()
    {
        yield return new WaitForSeconds(shootingCountDown);
        //stop all radio
        Debug.Log("Shooting!!");
        isShootingStory = true;
        previousChannelSource.Off();
        previousChannelSource = null;
        
        shootingChannel.SetTimeMusic();
        shootingChannel.On();
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
    
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    
}
