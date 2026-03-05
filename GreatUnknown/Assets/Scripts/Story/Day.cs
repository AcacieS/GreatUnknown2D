using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DayStoryInfo", menuName = "Scriptable Objects/DayStoryInfo")]
public class DayStoryInfo : ScriptableObject
{
    public Sprite[] faxStorySprites;
    public AudioClip[] radioStory;
    public AudioClip GetAudioClip(int index)
    {
        return radioStory[index];
    }
    public Sprite GetFaxStorySprite(int index)
    {
        return faxStorySprites[index];
    }

}
