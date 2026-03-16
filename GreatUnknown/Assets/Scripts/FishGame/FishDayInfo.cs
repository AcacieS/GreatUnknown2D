using UnityEngine;

[CreateAssetMenu(fileName = "FishDayInfo", menuName = "Scriptable Objects/FishDayInfo")]
public class FishDayInfo : ScriptableObject
{
    [SerializeField] private NbFishPerGame nbOfFish;
    [SerializeField] private int minNbOfMutatedFish;
    [Tooltip("be less than 100: Mutation likeliHood: index is nb mutation, value is percentage ")]
    [SerializeField] private MutationLikeliHood mutationLikeliHood;
    [SerializeField] private Percentage mutationChance;
    [Tooltip("Percentage is normal")]
    [SerializeField] private Percentage normalLikeliHood;
    [SerializeField] private Percentage normalEasyChance;
    public int GetRandomNbMutationPerFish()
    {
        return mutationLikeliHood.Roll();
    }
    public int GetMinNbMutatedFish()
    {
        return minNbOfMutatedFish;
    }
    public FishBPDifficulty GetIsMutationEasy(int nbMutation)
    {
        return mutationLikeliHood.GetIsMutationEasy(nbMutation);
    }
    public FishBPDifficulty GetIsNormalEasy()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll < normalEasyChance.GetPercentage())
        {
            return FishBPDifficulty.Easy;
        }
        else
        {
            return FishBPDifficulty.Hard;
        }
    }

    public bool GetIsMutated()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        return roll<mutationChance.GetPercentage();
    }
    public bool GetIsBaseNormal()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        return roll<normalLikeliHood.GetPercentage();
    }
    public int GetNbOfFish()
    {
        return nbOfFish.randomNbFish();
    }
    
}
