using TMPro;
using UnityEngine;

public class IsMutatedHandler : MonoBehaviour
{
    [SerializeField] private FishManagement fishManagement;
    [SerializeField] private FishSession session;
    [SerializeField] private TextMeshProUGUI fishTxt;

    public void ClickIsMutated()    => HandleChoice(true);
    public void ClickIsNotMutated() => HandleChoice(false);

    private void HandleChoice(bool playerSaysMutated)
    {
        if (fishManagement == null || session == null) return;

        Fish currentFish = fishManagement.GetCurrentFish();
        if (currentFish == null) return;

        bool correct = (currentFish.isMutated == playerSaysMutated);

        if (correct) {
            session.AddCorrect();
            fishTxt.color = Color.green;
            fishTxt.text = "Correctly identified fish";
        }
        else{
            fishTxt.color = Color.red;
            fishTxt.text = "Incorrectly identified fish";
            session.AddWrong();
        }

        //fishManagement.NextFish();
    }
}
