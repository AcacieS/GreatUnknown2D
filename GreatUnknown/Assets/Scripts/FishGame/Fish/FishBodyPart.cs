using UnityEngine;

[CreateAssetMenu(fileName = "FishPart", menuName = "Scriptable Objects/FishPart")]
public class FishBodyPart : ScriptableObject
{
    [SerializeField] public string description;
    [SerializeField] public Sprite bodyPartSprite;
    [SerializeField, Range(0f,100f)] private float percentage;
    [SerializeField] public bool isMutated;
    [SerializeField] public Restrictions restrictionFish;

}
