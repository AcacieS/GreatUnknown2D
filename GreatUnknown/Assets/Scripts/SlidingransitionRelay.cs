using UnityEngine;

public class SlidingTransitionRelay : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;

    public void OnSlidingTransitionComplete()
    {
        if (gameManagement == null)
        {
            Debug.LogError("GameManagement reference missing on SlidingTransitionRelay.");
            return;
        }

        gameManagement.OnSlidingTransitionComplete();
    }

    public void OnSlidingExitComplete()
    {
        if (gameManagement == null)
        {
            Debug.LogError("GameManagement reference missing on SlidingTransitionRelay.");
            return;
        }

        gameManagement.OnSlidingExitComplete();
    }
}