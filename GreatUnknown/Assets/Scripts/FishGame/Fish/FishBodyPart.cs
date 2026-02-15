using UnityEngine;

[CreateAssetMenu(fileName = "FishPart", menuName = "Scriptable Objects/FishPart")]
public class FishBodyPart : ScriptableObject
{
    [SerializeField] public string description;
    [SerializeField] public Sprite bodyPartSprite;
    [SerializeField] public bool isMutated;
    [SerializeField] public FishBPDifficulty difficulty;

}
