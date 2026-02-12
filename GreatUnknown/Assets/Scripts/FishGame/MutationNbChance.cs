using System;
using UnityEngine;

[Serializable]
public class MutationNbChance
{
    [SerializeField] private int mutationCount;
    [SerializeField] private Percentage percentage;
    public MutationNbChance()
    {
        
    }
    public MutationNbChance(int pMutationCount)
    {
        this.mutationCount = pMutationCount;
    }
    public int GetMutationCount()
    {
        return mutationCount;
    }
    public float GetPercentage()
    {
        return percentage.GetPercentage();
    }
}
