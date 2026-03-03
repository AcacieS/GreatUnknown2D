using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour, IClickable
{
    [SerializeField] private List<AudioClip> fishSounds;
    public void OnClick()
    {
        SoundManager.instance.PlaySound(
            fishSounds[Random.Range(0, fishSounds.Count)]);
    }
}
