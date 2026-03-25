using UnityEngine;
using UnityEditor;

public class AssetVerifier : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var guidHex in AssetDatabase.FindAssets("t:FishDaysInfo"))
        {
            GUID.TryParse(guidHex, out GUID guid);
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var fishDaysInfo = AssetDatabase.LoadAssetByGUID<FishDaysInfo>(guid);
            if (!fishDaysInfo.verifyFishDaysInfoIntegrity(out var whatWentWrong))
            Debug.LogError("[AssetVerifier] Problem in asset " + path + "\n" + whatWentWrong);
        }
    }
}
