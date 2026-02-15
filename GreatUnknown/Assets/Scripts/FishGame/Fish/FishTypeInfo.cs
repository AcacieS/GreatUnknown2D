using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FishTypeInfo", menuName = "Scriptable Objects/FishTypeInfo")]
public class FishTypeInfo : ScriptableObject
{
    public string fishName;
    public List<CategoryFishBodyPart> categoriesFishLayer;
    public int GetNbPossibleMutation()
    {
        int nbPossibleMutation = 0;
        foreach(CategoryFishBodyPart categoryFishBodyPart in categoriesFishLayer)
        {
            if (categoryFishBodyPart.HasMutation())
            {
                nbPossibleMutation++;
            }
        }
        return nbPossibleMutation;
    }
}
