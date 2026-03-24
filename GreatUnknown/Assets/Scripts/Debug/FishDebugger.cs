using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(FishManagement))]
public class FishDebugger : MonoBehaviour
{
    private FishManagement _fm;

    void Awake()
    {
        _fm = GetComponent<FishManagement>();
    }

    [ContextMenu("Regenerate Fish Body List")]
    void GenerateFish()
    {
        foreach(FishTypeInfo fishTypeInfo in _fm.GetFishDaysInfo().GetFishInfos())
        {
            foreach(CategoryFishBodyPart catFishBodyPart in fishTypeInfo.categoriesFishLayer)
            {
                Undo.RecordObject(catFishBodyPart, "Recorded fish body part");
                catFishBodyPart.InitializeFishParts();
            }
            
        }
    }
}
