using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDayInfo", menuName = "Scriptable Objects/FishDayInfo")]
public class FishDayInfo : ScriptableObject
{
    [SerializeField] private NbFishPerGame nbOfFish;
    [Tooltip("be less than 100: Mutation likeliHood: index is nb mutation, value is percentage ")]
    [SerializeField] private MutationLikeliHood mutationLikeliHood;
    [SerializeField] private Percentage mutationChance;
    [SerializeField] private Percentage normalLikeliHood;

    public int GetRandomNbMutationPerFish()
    {
        return mutationLikeliHood.Roll();
    }
    public bool GetIsMutated()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        return roll<mutationChance.GetPercentage();
    }
    public bool GetIsSpecialNormal()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        return roll<normalLikeliHood.GetPercentage();
    }
    public int GetNbOfFish()
    {
        return nbOfFish.randomNbFish();
    }
    
}
