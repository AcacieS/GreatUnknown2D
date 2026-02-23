using UnityEngine;
using System;
using System.Collections.Generic;
using System.Data.Common;

[Serializable] 
public class Fish{
    public bool isMutated;
    public FishTypeInfo fishType;
    public Dictionary<CategoryFishBodyPart, FishBodyPart> fishBodyParts;
    public List<GameObject> fishBodyPartsGameObj;
    public Fish(FishTypeInfo fishTypeInfo, bool isMutated)
    {
        fishType = fishTypeInfo;
        fishBodyParts = new Dictionary<CategoryFishBodyPart, FishBodyPart>();
        fishBodyPartsGameObj = new List<GameObject>();
        this.isMutated = isMutated;
    }
    
    public void AddFishBodyPart(CategoryFishBodyPart fishCategoryBodyPart, FishBodyPart fishBodyPart)
    {
        if (fishBodyParts.ContainsKey(fishCategoryBodyPart))
        {
            Debug.LogError("fishCategoryBody Part is already duplicated: "+fishCategoryBodyPart);
            Debug.LogError("fish first : "+fishBodyParts[fishCategoryBodyPart]+", now want to add: "+fishBodyPart);
        }
        else
        {
            fishBodyParts.Add(fishCategoryBodyPart, fishBodyPart);
        }
        
    }
    public void AddFishBodyPartGameObj(GameObject fishBodyPart)
    {
        fishBodyPartsGameObj.Add(fishBodyPart);
    }
    public bool GetIsMutated()
    {
        return isMutated;
    }
    public Dictionary<CategoryFishBodyPart, FishBodyPart> GetFishBodyParts()
    {
        return fishBodyParts;
    }
    public List<GameObject> GetFishBodyPartsGameObj()
    {
        return fishBodyPartsGameObj;
    }
    public FishTypeInfo GetFishType()
    {
        return fishType;
    }
}