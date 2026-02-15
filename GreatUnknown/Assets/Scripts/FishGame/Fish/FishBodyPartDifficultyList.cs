using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FishBodyPartDifficultyList
{
    [SerializeField] private List<FishBodyPart> EasyFishParts;
    [SerializeField] private List<FishBodyPart> HardFishParts;
    public List<FishBodyPart> GetFishParts(FishBPDifficulty difficulty)
    {
        if(difficulty == FishBPDifficulty.Easy)
        {
            return EasyFishParts;
        }
        else //HARD
        {
            return HardFishParts;
        }
    }
    public int GetNbFishParts(FishBPDifficulty difficulty)
    {
        if(difficulty == FishBPDifficulty.Easy)
        {
            return EasyFishParts.Count;
        }
        else //HARD
        {
            return HardFishParts.Count;
        }
    }

}
