using UnityEngine;

public class IceSlidingGameSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] slidingGames;

    private GameObject currentGame;
    private int currentDayIndex = -1;

    public void ActivateForDay(int dayIndex)
    {
        if (slidingGames == null || slidingGames.Length == 0)
        {
            Debug.LogError("[IceSlidingGameSwitcher] No sliding games assigned.");
            return;
        }

        if (dayIndex < 0 || dayIndex >= slidingGames.Length)
        {
            Debug.LogError($"[IceSlidingGameSwitcher] Day index {dayIndex} is out of range.");
            return;
        }

        DeactivateAll();

        currentGame = slidingGames[dayIndex];
        currentDayIndex = dayIndex;

        if (currentGame != null)
            currentGame.SetActive(true);

        Debug.Log($"[IceSlidingGameSwitcher] Activated sliding game for day index {dayIndex}.");
    }

    public void DeactivateCurrent()
    {
        if (currentGame != null)
            currentGame.SetActive(false);

        currentGame = null;
        currentDayIndex = -1;
    }

    public void DeactivateAll()
    {
        if (slidingGames == null) return;

        for (int i = 0; i < slidingGames.Length; i++)
        {
            if (slidingGames[i] != null)
                slidingGames[i].SetActive(false);
        }

        currentGame = null;
        currentDayIndex = -1;
    }

    public GameObject GetCurrentGame()
    {
        return currentGame;
    }

    public GameObject GetSlidingGameForDay(int dayIndex)
    {
        if (slidingGames == null || dayIndex < 0 || dayIndex >= slidingGames.Length)
            return null;

        return slidingGames[dayIndex];
    }
}