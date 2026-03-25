using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        PlayRandomWetSound();
    }
    public void PlayRandomWetSound()
    {
        int wetFishIndex = Random.Range(1, 3);

        if (SoundManager.instance == null)
        {
            Debug.LogError("FishSound: SoundManager.instance is null.");
            return;
        }

        SoundManager.instance.PlaySound("fishWet" + wetFishIndex);
    }
}
