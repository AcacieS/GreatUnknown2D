using UnityEngine;
using System;

[Serializable] 
public class Fish{
    public bool isMutated;
    public Sprite sprite;
    public Fish(Sprite sprite, bool isMutated)
    {
        this.sprite = sprite;
        this.isMutated = isMutated;
    }
    public bool GetIsMutated()
    {
        return isMutated;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
}