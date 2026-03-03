using UnityEngine;

public class FishGetOut : MonoBehaviour, IDropable
{
    public void OnDropEvent(GameObject droppedObject)
    {
        if (droppedObject.GetComponent<FishState>() == null)
        {
            return;
        }
        
        bool choiceIsMutated = droppedObject.GetComponent<FishState>().GetChoiceIsMutated();
        if (choiceIsMutated)
        {
            FishManagement.Instance.ClickIsMutated();
        }
        else
        {
            FishManagement.Instance.ClickIsNotMutated();
        }
    }
}
