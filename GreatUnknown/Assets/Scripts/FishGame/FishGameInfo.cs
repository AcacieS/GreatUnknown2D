using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable] 
public class FishGameInfo
{
    [SerializeField] private List<FishTypeInfo> fishesType;
    [SerializeField] private List<NbFishPerGame> nbOfFishPerGames;
    [SerializeField] private List<MutatedPercentage> percentageMutatedFishes;

    public int GetNbGame(){
        return nbOfFishPerGames.Count;
    }
    public FishGameInfo()
    {
        
    }
    public float GetPercentageMutatedFishes(int currentGameIndex)
    {
        return percentageMutatedFishes[currentGameIndex].GetPercentage();
    }
    public int GetNbOfFishPerGame(int currentGameIndex)
    {
        return nbOfFishPerGames[currentGameIndex].randomNbFish();
    }
    public FishTypeInfo GetFishInfo(int fishIndex)
    {
        return fishesType[fishIndex];
    }
    public int GetTotalFishType()
    {
        return fishesType.Count;
    }
    public void Validate()
    {
        if (nbOfFishPerGames.Count != percentageMutatedFishes.Count)
        {
            Debug.LogWarning("FishGameInfo lists were not same lengths.");
        }
    }
    
}