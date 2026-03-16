using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DayStoryInfo", menuName = "Scriptable Objects/DayStoryInfo")]
public class DayStoryInfo : ScriptableObject
{
    public Sprite[] faxStorySprites;
    public AudioInfo[] radioStory;
    public AudioInfo GetRadioStory(int index)
    {
        if(index >= faxStorySprites.Length)
        {
            return null;
        }
        return radioStory[index];
    }
    public Sprite GetFaxStorySprite(int index)
    {
        if(index >= faxStorySprites.Length)
        {
            return null;
        }

        return faxStorySprites[index];
    }

}
