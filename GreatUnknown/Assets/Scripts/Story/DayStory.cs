using UnityEngine;

public class DayStory
{
    private DayStoryInfo dayStoryInfo;
    private int faxStoryIndex = 0;
    private int radioStoryIndex = 0;
    public void Reset()
    {
        faxStoryIndex = 0;
        radioStoryIndex = 0;
    }
    public Sprite GetFaxStory()
    {
        Sprite currentFaxStory = dayStoryInfo.GetFaxStorySprite(faxStoryIndex);
        faxStoryIndex++;
        return currentFaxStory;
    }
    public AudioInfo GetRadioStory()
    {
        AudioInfo currentRadioStory = dayStoryInfo.GetRadioStory(radioStoryIndex);
        radioStoryIndex++;
        return currentRadioStory;
    }
}