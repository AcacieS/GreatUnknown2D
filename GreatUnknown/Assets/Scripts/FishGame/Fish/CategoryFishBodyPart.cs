using UnityEngine;

[CreateAssetMenu(fileName = "CategoryFishBodyPart", menuName = "Scriptable Objects/CategoryFishBodyPart")]
public class CategoryFishBodyPart : ScriptableObject
{
    [SerializeField] public string fishLayerName;
    [SerializeField] public FishBodyPart baseBodyPartFish; //optional
    [SerializeField] public bool isOptional;
    [SerializeField] public FishBodyPartDifficultyList fishPartsMutated;
    [SerializeField] public FishBodyPartDifficultyList fishPartsNormal;
    [SerializeField] public Restrictions CategoryRestriction;
    [SerializeField, Range(0f,100f)] private float CategoryPercentage;
    [SerializeField] public FishSortingLayer sortingLayer;

    public int GetNbPossibleMutation()
    {
        int nbFishPartsMutatedEasy = fishPartsMutated.GetFishParts(FishBPDifficulty.Easy).Count;
        int nbFishPartsMutatedHard = fishPartsMutated.GetFishParts(FishBPDifficulty.Hard).Count;
        return nbFishPartsMutatedEasy + nbFishPartsMutatedHard;
    }
    public int GetNbPossibleSpecialNormal()
    {
        int nbFishPartsNormalEasy = fishPartsNormal.GetFishParts(FishBPDifficulty.Easy).Count;
        int nbFishPartsNormalHard = fishPartsNormal.GetFishParts(FishBPDifficulty.Hard).Count;
        return nbFishPartsNormalEasy + nbFishPartsNormalHard;
    }
    public bool HasMutation()
    {
        if(GetNbPossibleMutation()==0)
        {
            return false;
        }
        return true;
    }

}
