using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDaysInfo", menuName = "Scriptable Objects/FishDaysInfo")]
public class FishDaysInfo : ScriptableObject
{
    [SerializeField] private List<FishTypeInfo> fishesType;
    [SerializeField] private List<FishDayInfo> fishDaysInfo = new List<FishDayInfo>();

    public int GetNbGame(){
        return fishDaysInfo.Count;
    }
    public FishDayInfo GetCurrentFishDayInfo(int currentDay)
    {
        return fishDaysInfo[currentDay];
    }
    public int GetRandomNbMutationPerFish(int currentDay)
    {
        return fishDaysInfo[currentDay].GetRandomNbMutationPerFish();
    }
    public bool GetIsMutated(int currentDay)
    {
        return fishDaysInfo[currentDay].GetIsMutated();
    }
    public bool GetIsBaseNormal(int currentDay)
    {
        return fishDaysInfo[currentDay].GetIsBaseNormal();
    }
    public int GetNbOfFishPerGame(int currentDay)
    {
        return fishDaysInfo[currentDay].GetNbOfFish();
    }
    public FishTypeInfo GetFishInfo(int fishIndex)
    {
        return fishesType[fishIndex];
    }
    public List<FishTypeInfo> GetFishInfos()
    {
        return fishesType;
    }
    public int GetTotalFishType()
    {
        return fishesType.Count;
    }
    
    public bool verifyFishDaysInfoIntegrity(out string errorReason)
    {
        bool correct = true;
        errorReason = "Could Not Ensure FishDaysInfo integrity\n";
        foreach (FishTypeInfo type in fishesType)
        {
            errorReason += "In Type " + type.fishName + "\n";
            foreach (CategoryFishBodyPart cat in type.categoriesFishLayer)
            {
                bool listCheck;
                string listCheckError;
                errorReason += "\tIn Category " + cat.fishLayerName + "\n";

                listCheck = verifyFishBodyPartList(cat.fishPartsMutated.GetFishParts(FishBPDifficulty.Easy), out listCheckError);
                if (!listCheck) { correct = false; errorReason += "\t\tWhile Checking Mutated Easy Parts: " + listCheckError; }

                listCheck = verifyFishBodyPartList(cat.fishPartsMutated.GetFishParts(FishBPDifficulty.Hard), out listCheckError);
                if (!listCheck) { correct = false; errorReason += "\t\tWhile Checking Mutated Hard Parts: " + listCheckError; }

                listCheck = verifyFishBodyPartList(cat.fishPartsNormal.GetFishParts(FishBPDifficulty.Easy), out listCheckError);
                if (!listCheck) { correct = false; errorReason += "\t\tWhile Checking Normal Easy Parts: " + listCheckError; }

                listCheck = verifyFishBodyPartList(cat.fishPartsNormal.GetFishParts(FishBPDifficulty.Hard), out listCheckError);
                if (!listCheck) { correct = false; errorReason += "\t\tWhile Checking Normal Hard Parts: " + listCheckError; }

                listCheck = verifyFishBodyPartList(cat.fishParts, out listCheckError);
                if (!listCheck) { correct = false; errorReason += "\t\tWhile Checking Fish Parts" + listCheckError; }
            } 
        }
        return correct;
    }

    public bool verifyFishBodyPartList(List<FishBodyPart> list, out string errorReason)
    {
        bool correct = true;
        errorReason = null;
        if (list == null)
        {
            errorReason = "FishBodyPart List is null\n";
            return false;
        }
        foreach (FishBodyPart part in list)
        {
            if (part == null) correct = false;
        }
        if (!correct) errorReason = "FishBodyPart List contains null(s)";
        return correct;
    }
}
