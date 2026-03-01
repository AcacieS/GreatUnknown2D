using UnityEngine;
using UnityEngine.EventSystems;

public interface IDropable
{
    void OnDropEvent(GameObject droppedObject);
}

