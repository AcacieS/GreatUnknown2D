using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDaysInfo", menuName = "Scriptable Objects/FishDaysInfo")]
public class FishDaysInfo : ScriptableObject
{
    [SerializeField] private List<FishTypeInfo> fishesType;
    [SerializeField] private List<FishDayInfo> fishDaysInfo = new List<FishDayInfo>();

    public int GetNbGame(){
        return fishDaysInfo.Count;
    }
    public FishDayInfo GetCurrentFishDayInfo(int currentDay)
    {
        return fishDaysInfo[currentDay];
    }
    public int GetRandomNbMutationPerFish(int currentDay)
    {
        return fishDaysInfo[currentDay].GetRandomNbMutationPerFish();
    }
    public bool GetIsMutated(int currentDay)
    {
        return fishDaysInfo[currentDay].GetIsMutated();
    }
    public bool GetIsBaseNormal(int currentDay)
    {
        return fishDaysInfo[currentDay].GetIsBaseNormal();
    }
    public int GetNbOfFishPerGame(int currentDay)
    {
        return fishDaysInfo[currentDay].GetNbOfFish();
    }
    public FishTypeInfo GetFishInfo(int fishIndex)
    {
        return fishesType[fishIndex];
    }
    public List<FishTypeInfo> GetFishInfos()
    {
        return fishesType;
    }
    public int GetTotalFishType()
    {
        return fishesType.Count;
    }
    
}
