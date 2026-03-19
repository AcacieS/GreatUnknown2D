using UnityEngine;

public class DayDebugTools : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;

    private void Awake()
    {
        if (gameManagement == null)
            gameManagement = GetComponent<GameManagement>();

        if (gameManagement == null)
            gameManagement = FindFirstObjectByType<GameManagement>();
    }

    [ContextMenu("Debug/Log Current Day")]
    public void LogCurrentDay()
    {
        if (gameManagement == null)
        {
            Debug.LogError("[DayDebugTools] GameManagement reference missing.");
            return;
        }

        int internalDay = GameManagement.nbDaysPassed;
        int humanDay = internalDay + 1;

        Debug.Log($"[DayDebugTools] Internal day index = {internalDay}, Human-readable day = {humanDay}");
    }

    [ContextMenu("Debug/Advance To Next Day")]
    public void AdvanceToNextDay()
    {
        if (gameManagement == null)
        {
            Debug.LogError("[DayDebugTools] GameManagement reference missing.");
            return;
        }

        int beforeInternal = GameManagement.nbDaysPassed;
        int beforeHuman = beforeInternal + 1;

        Debug.Log($"[DayDebugTools] Before advance -> Internal = {beforeInternal}, Human day = {beforeHuman}");

        gameManagement.isFishGameFinished = true;
        gameManagement.isSlidingGameFinished = true;
        gameManagement.NextDay();

        int afterInternal = GameManagement.nbDaysPassed;
        int afterHuman = afterInternal + 1;

        Debug.Log($"[DayDebugTools] After advance -> Internal = {afterInternal}, Human day = {afterHuman}");
    }

    [ContextMenu("Debug/Log Whether LastDay Should Trigger")]
    public void LogLastDayTriggerState()
    {
        int internalDay = GameManagement.nbDaysPassed;
        int humanDay = internalDay + 1;

        bool willTriggerCurrentCode = internalDay == 5;

        Debug.Log(
            $"[DayDebugTools] Internal = {internalDay}, Human day = {humanDay}, " +
            $"Current code would trigger LastDay = {willTriggerCurrentCode} " +
            $"(because SpecialEventDay uses case 5)."
        );
    }
}