using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour, IClickable
{
public void OnClick()
{
    int wetFishIndex = Random.Range(1, 3);

    if (SoundManager.instance == null)
    {
        Debug.LogWarning("FishSound: SoundManager.instance is null.");
        return;
    }

    SoundManager.instance.PlaySound("fishWet" + wetFishIndex);
}
}
