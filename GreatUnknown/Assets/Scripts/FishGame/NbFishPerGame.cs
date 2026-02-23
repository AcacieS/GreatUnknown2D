using UnityEngine;
using System;

[Serializable] 
public class NbFishPerGame
{
    [SerializeField] private int minNbOfFish;
    [SerializeField] private int maxNbOfFish;
    public NbFishPerGame()
    {
        
    }

    public int randomNbFish()
    {
        return UnityEngine.Random.Range(minNbOfFish, maxNbOfFish+1);
    }
}