using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MutationLikeliHood
{
    [SerializeField]
    private List<MutationNbChance> mutationsNbChance;
    public FishBPDifficulty GetIsMutationEasy(int nbMutation)
    {
        foreach(MutationNbChance mutationNbChance in mutationsNbChance)
        {
            if (mutationNbChance.GetMutationCount() == nbMutation)
            {
                return mutationNbChance.GetIsEasy(); 
            }
        }
        Debug.LogError("Not found nb mutation");
        return FishBPDifficulty.Easy;
    }
    public int Roll()
    {
        float total = 0f;
        foreach (var e in mutationsNbChance)
            total += e.GetPercentage();

        float roll = UnityEngine.Random.Range(0f, total);
        float acc = 0f;

        foreach (var e in mutationsNbChance)
        {
            acc += e.GetPercentage();
            if (roll <= acc)
                return e.GetMutationCount();
        }

        return mutationsNbChance[^1].GetMutationCount();
    }
    public void Validate()
    {
        float total = 0f;
        foreach (var e in mutationsNbChance)
            total += e.GetPercentage();
        if (total != 100)
        {
            Debug.LogWarning("Mutation LikeliHood not 100% ");
        }
    }

}