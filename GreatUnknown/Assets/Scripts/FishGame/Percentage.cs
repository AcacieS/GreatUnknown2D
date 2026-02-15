using UnityEngine;
using System;

[Serializable] 
public class Percentage
{
    [SerializeField, Range(0f,100f)] private float percentage;
    public float GetPercentage()
    {
        return percentage;
    }
}