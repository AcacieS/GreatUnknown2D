using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CategoryFishBodyPart", menuName = "Scriptable Objects/CategoryFishBodyPart")]
public class CategoryFishBodyPart : ScriptableObject
{
    [SerializeField] public string fishLayerName;
    [SerializeField] public FishBodyPart baseBodyPartFish; //optional
    [SerializeField] public bool isOptional;
    [SerializeField] public List<FishBodyPart> fishParts;
    [SerializeField] public FishBodyPartDifficultyList fishPartsMutated;
    [SerializeField] public FishBodyPartDifficultyList fishPartsNormal;
    [SerializeField] public Restrictions CategoryRestriction;
    [SerializeField, Range(0f,100f)] private float CategoryPercentage;
    [SerializeField] public FishSortingLayer sortingLayer;
    public void InitializeFishParts()
    {
        fishPartsNormal = new FishBodyPartDifficultyList();
        fishPartsMutated = new FishBodyPartDifficultyList();
        foreach( FishBodyPart fishBodyPart in fishParts)
        {
            if (fishBodyPart.isMutated)
            {
                if(fishBodyPart.difficulty == FishBPDifficulty.Easy)
                {
                    fishPartsMutated.AddEasyFishPart(fishBodyPart);
                }
                else //HARD
                {
                    fishPartsMutated.AddHardFishPart(fishBodyPart);
                }
            }
            else //NOT mutated
            {
                if(fishBodyPart.difficulty == FishBPDifficulty.Easy)
                {
                    fishPartsNormal.AddEasyFishPart(fishBodyPart);
                }
                else //HARD
                {
                    fishPartsNormal.AddHardFishPart(fishBodyPart);
                }
            }
        }
    }
    public int GetNbPossibleMutation()
    {
        int nbFishPartsMutatedEasy = fishPartsMutated.GetFishParts(FishBPDifficulty.Easy).Count;
        int nbFishPartsMutatedHard = fishPartsMutated.GetFishParts(FishBPDifficulty.Hard).Count;
        return nbFishPartsMutatedEasy + nbFishPartsMutatedHard;
    }
    public int GetNbPossibleSpecialNormal()
    {
        int nbFishPartsNormalEasy = fishPartsNormal.GetFishParts(FishBPDifficulty.Easy).Count;
        int nbFishPartsNormalHard = fishPartsNormal.GetFishParts(FishBPDifficulty.Hard).Count;
        return nbFishPartsNormalEasy + nbFishPartsNormalHard;
    }
    public bool HasMutation()
    {
        if(GetNbPossibleMutation()==0)
        {
            return false;
        }
        return true;
    }

}
