using UnityEngine;

public class IceSlidingGameSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] slidingGames;

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

        for (int i = 0; i < slidingGames.Length; i++)
        {
            if (slidingGames[i] != null)
                slidingGames[i].SetActive(i == dayIndex);
        }

        Debug.Log($"[IceSlidingGameSwitcher] Activated sliding game for day index {dayIndex}.");
    }

    public GameObject GetSlidingGameForDay(int dayIndex)
    {
        if (slidingGames == null || dayIndex < 0 || dayIndex >= slidingGames.Length)
            return null;

        return slidingGames[dayIndex];
    }
}