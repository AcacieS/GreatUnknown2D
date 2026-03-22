using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        int wetFishIndex = Random.Range(1, 3);
        SoundManager.instance.PlaySound("fishWet"+wetFishIndex);
    }
}
