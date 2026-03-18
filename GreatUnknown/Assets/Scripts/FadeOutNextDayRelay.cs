using Unity.VisualScripting;
using UnityEngine;

public class FadeOutNextDayRelay : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeOutNextDay()
    {
        gameManagement.OnDayFadeComplete();
    }
}
