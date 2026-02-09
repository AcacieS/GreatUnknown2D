using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryFishBodyPart", menuName = "Scriptable Objects/CategoryFishBodyPart")]
public class CategoryFishBodyPart : ScriptableObject
{
    [SerializeField] public string fishLayerName;
    [SerializeField] public FishBodyPart baseBodyPartFish; //optional
    [SerializeField] public bool isOptional;
    [SerializeField] public List<FishBodyPart> fishPartsMutated;
    [SerializeField] public List<FishBodyPart> fishPartsNormal;
    [SerializeField] public Restrictions CategoryRestriction;
    [SerializeField, Range(0f,100f)] private float CategoryPercentage;
    [SerializeField] public FishSortingLayer sortingLayer;

}
