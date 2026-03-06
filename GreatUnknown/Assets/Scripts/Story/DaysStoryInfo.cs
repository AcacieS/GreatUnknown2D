using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DaysStoryInfo", menuName = "Scriptable Objects/DaysStoryInfo")]
public class DaysStoryInfo : ScriptableObject
{
    public DayStoryInfo[] ListDayStoryInfo;
    public DayStoryInfo GetDayStoryInfo(int day)
    {
        return ListDayStoryInfo[day];
    }

}
