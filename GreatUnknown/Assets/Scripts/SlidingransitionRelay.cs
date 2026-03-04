using UnityEngine;

public class SlidingTransitionRelay : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;

    // Called by Animation Event
    public void OnSlidingTransitionComplete()
    {
        if (gameManagement == null)
        {
            Debug.LogError("GameManagement reference missing on SlidingTransitionRelay.");
            return;
        }

        gameManagement.OnSlidingTransitionComplete();
    }
}