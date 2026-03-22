using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private string mainScene = "Stefa";
    [SerializeField] private Button[] dayButtons;

    void Start()
    {
        int daysCompleted = SaveSystem.LoadProgress();

        for (int i = 0; i < dayButtons.Length; i++)
        {
            int dayIndex = i;
            bool isUnlocked = i <= daysCompleted;
            
            dayButtons[i].interactable = isUnlocked;

            if (dayButtons[i].TryGetComponent<HoverButton>(out var hover))
            {
                hover.enabled = isUnlocked;
            }

            if (isUnlocked)
            {
                // Use Lambda notation
                //  even if something bypasses interactable, nothing happens
                dayButtons[i].onClick.AddListener(() => StartFromDay(dayIndex));
            }
        }
    }
    void StartFromDay(int dayIndex)
    {
        GameManagement.nbDaysPassed = dayIndex;
        SaveSystem.SaveProgress(dayIndex);
        SceneManager.LoadScene(mainScene);
    }
}
