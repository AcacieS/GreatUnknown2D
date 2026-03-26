using UnityEditor;

public class EditorTWTWUtils
{
    [MenuItem("To Wound The Waters/Regenerate Fish Body Parts")]
    public static void RegenerateFishBodyParts()
    {
        foreach (var guidHex in AssetDatabase.FindAssets("t:FishDaysInfo"))
        {
            GUID.TryParse(guidHex, out GUID guid);
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var fishDaysInfo = AssetDatabase.LoadAssetByGUID<FishDaysInfo>(guid);
            foreach(FishTypeInfo fishTypeInfo in fishDaysInfo.GetFishInfos())
            {
                foreach(CategoryFishBodyPart catFishBodyPart in fishTypeInfo.categoriesFishLayer)
                {
                    Undo.RecordObject(catFishBodyPart, "Regenerate Fish Body Parts");
                    catFishBodyPart.InitializeFishParts();
                }
            }
        }
    }
}
