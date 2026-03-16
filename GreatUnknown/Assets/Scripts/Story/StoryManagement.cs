using System.Collections.Generic;
using UnityEngine;

public class StoryManagement : MonoBehaviour
{
    public static StoryManagement Instance {get; private set;}
    public List<DayStory> dayStories = new List<DayStory>();
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private FaxMachine faxMachine;
    int currentDay = 0;
    private DayStory currentDayStory;
    public void Start()
    {
        
    }
    public void PlayRadio()
    {
        AudioInfo currentRadioStory =currentDayStory.GetRadioStory();
        //TODO: idk
        radioSource.PlayOneShot(currentRadioStory.soundClip);
    }
    public void CallFaxMachine()
    {
        //faxMachine.NewFaxMessage(currentDayStory.GetFaxStory());
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
