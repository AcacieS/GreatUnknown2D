using UnityEngine;

public class SlidingDebugController : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;

    [Header("Debug Day Selection")]
    [SerializeField] private int debugDay = 0;

    [ContextMenu("Open Sliding Game (Debug)")]
    public void OpenSlidingGameDebug()
    {
        if (gameManagement == null)
        {
            Debug.LogError("GameManagement not assigned.");
            return;
        }

        // Clamp to valid range
        int maxDays = gameManagement.GetNbDayLeft() + gameManagement.GetNbDayPassed() + 1;
        debugDay = Mathf.Clamp(debugDay, 0, maxDays - 1);

        // Force the day
        GameManagement.nbDaysPassed = debugDay;

        Debug.Log($"[DEBUG] Forcing day = {debugDay}");

        // Pretend fish game is done so sliding can start
        gameManagement.IsFishGameFinished = true;

        // Trigger normal flow (uses animator + relay)
        gameManagement.StartSlidingGame();
    }
}