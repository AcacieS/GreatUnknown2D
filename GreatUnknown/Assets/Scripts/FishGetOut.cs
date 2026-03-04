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
        Debug.Log("Choice is mutated: "+choiceIsMutated);
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
