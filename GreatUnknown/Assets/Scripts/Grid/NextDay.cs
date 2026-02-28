using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDayButton : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public void GoToNextDay()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("NextDayButton: No scene name assigned.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}