using UnityEngine;
using UnityEditor;

public class Verifier
{
    [InitializeOnLoadMethod]
    public static void VerifyFishDaysInfo()
    {
        //// TODO: Locate fishDaysInfo
        //var fishDaysInfo = ;
        Debug.LogError("[Verifier] TODO: check if fish are entered correctly");
        //// Check that fishDaysInfo is properly formatted
        //if (!fishDaysInfo.verifyFishDaysInfoIntegrity(out var whatWentWrong))
        //    Debug.LogError("[Verifier] " + whatWentWrong);
    }
}
