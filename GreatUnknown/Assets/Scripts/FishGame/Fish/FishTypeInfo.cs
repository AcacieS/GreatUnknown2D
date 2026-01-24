using UnityEngine;

[CreateAssetMenu(fileName = "FishTypeInfo", menuName = "Scriptable Objects/FishTypeInfo")]
public class FishTypeInfo : ScriptableObject
{
    public string fishName;
    public Sprite[] normalFishes;
    public Sprite[] mutatedFishes;
}
