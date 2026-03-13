using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RadiosStoryInfo", menuName = "Scriptable Objects/RadiosStoryInfo")]
public class RadiosStoryInfo : ScriptableObject
{
    [SerializeField] private List<RadioStoryInfo> radioStoryInfos= new List<RadioStoryInfo>();
    public RadioStoryInfo GetRadioStory(string id)
    {
        foreach (var radioStoryInfo in radioStoryInfos)
        {
            if(id == radioStoryInfo.id)
            {
                return radioStoryInfo;
            }
        }
        Debug.LogError("this radio id doesn't exist");
        return null;
    }
}