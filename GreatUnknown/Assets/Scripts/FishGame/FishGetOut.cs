using UnityEngine;

public class FishGetOut : MonoBehaviour, IDropable
{
    public void OnDropEvent(GameObject droppedObject)
    {
        if (droppedObject.GetComponent<FishState>() == null)
        {
            Debug.Log("Dropped object is not a fish");
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
