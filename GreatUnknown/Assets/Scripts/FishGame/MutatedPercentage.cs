using UnityEngine;
using System;

[Serializable] 
public class MutatedPercentage
{
    [SerializeField, Range(0f,100f)] private float percentage;
    public MutatedPercentage()
    {
        
    }
    public float GetPercentage()
    {
        return percentage;
    }
}