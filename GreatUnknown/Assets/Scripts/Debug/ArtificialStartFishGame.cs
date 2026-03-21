using UnityEngine;

public class FishGameDebugStarter : MonoBehaviour
{
    [SerializeField] private GameManagement fishGameController;


    private void Awake()
    {
        // Optional auto-assign if not manually set
        if (fishGameController == null)
        {
            fishGameController = FindFirstObjectByType<GameManagement>();
        }
    }

    [ContextMenu("Force Start Fish Game")]
    public void ForceStartFishGame()
    {
        if (fishGameController == null)
        {
            Debug.LogError("FishGameController reference missing.");
            return;
        }

        fishGameController.isFishGameFinished = true;
        fishGameController.StartSlidingGame();
    

        Debug.Log("Fish game artificially started.");
    }

    [ContextMenu("ExitThatH")]

    public  void ForceExitSlidingGame()
    {
        if(fishGameController == null)
        {
            Debug.LogError("FishGameController reference missing.");
            return;
        }

        fishGameController.ExitSlidingGame();
    }
}