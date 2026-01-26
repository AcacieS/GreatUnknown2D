using UnityEngine;

public class IsMutatedHandler : MonoBehaviour
{
    [SerializeField] private FishManagement fishManagement;

    // This method is wired to ClickHandler → Clicked (GameObject)
    public void HandleMutationChoice(GameObject clickedButton)
    {
        // 1. Get current fish
        Fish currentFish = fishManagement.GetCurrentFish();
        if (currentFish == null)
            return;

        bool isMutated = currentFish.isMutated;

        // 2. Check which button was clicked
        if (clickedButton.name == "ButtonIsMutated")
        {
            if (isMutated)
            {
                fishManagement.NextFish(true);
            }
        }
        else if (clickedButton.name == "ButtonIsNotMutated")
        {
            if (!isMutated)
            {
                fishManagement.NextFish(false);
            }
        }
    }
}
