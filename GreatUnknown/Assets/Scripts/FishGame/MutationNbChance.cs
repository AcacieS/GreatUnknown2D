using System;
using UnityEngine;

[Serializable]
public class MutationNbChance
{
    [SerializeField] private int mutationCount;
    [Tooltip("Percentage to be this number of mutation")]
    [SerializeField, Range(0f,100f)] private float percentage;
    [Tooltip("Percentage of easiness (rest: hardness)")]
    [SerializeField, Range(0f,100f)] private float easiness;
    public MutationNbChance()
    {
        
    }
    public float GetDifficulty()
    {
        return easiness;
    }
    public FishBPDifficulty GetIsEasy()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll < easiness)
        {
            return FishBPDifficulty.Easy;
        }
        return FishBPDifficulty.Hard;
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
        return percentage;
    }
}
