using UnityEngine;

public class FadeOutFired : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnFadeComplete()
    {
        gameObject.SetActive(false);
        GameManagement.Instance.ResetDay();
    }
}