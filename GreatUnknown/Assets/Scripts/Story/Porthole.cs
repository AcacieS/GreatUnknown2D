using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Porthole")]
class Porthole : ScriptableObject
{
    [SerializeField] public int day;
    [SerializeField] public Sprite sprite;
}