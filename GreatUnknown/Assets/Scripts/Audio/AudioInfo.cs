using UnityEngine;

[CreateAssetMenu(fileName = "AudioInfo", menuName = "Scriptable Objects/AudioInfo")]
public class AudioInfo : ScriptableObject
{
    public AudioClip soundClip;
    [SerializeField, Range(0f,1f)] public float volume = 1f;
    public bool isLooping = false;
}
