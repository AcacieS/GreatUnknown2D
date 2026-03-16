using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RadioStoryInfo", menuName = "Scriptable Objects/RadioStoryInfo")]
public class RadioStoryInfo : ScriptableObject
{
    public string id;
    public AudioInfo radioAudio;
}