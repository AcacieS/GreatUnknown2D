using Unity.VisualScripting;
using UnityEngine;

public class FadeOutNextDayRelay : MonoBehaviour
{
    public void FadeOutNextDay()
    {
        GameManagement.Instance.OnDayFadeComplete();
        gameObject.SetActive(false);
    }
}
