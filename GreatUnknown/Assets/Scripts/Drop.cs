using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Drop: MonoBehaviour, IDropHandler
{
    private IDropable _dropable;
    [SerializeField] private UnityEvent onDropEvent;
    private void Awake()
    {
        _dropable = GetComponent<IDropable>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped on Drop Zone");
        if(eventData.pointerDrag != null)
        {
            //needs verify if is the one you want;
            _dropable?.OnDropEvent(eventData.pointerDrag);
            onDropEvent?.Invoke();
            eventData.pointerDrag.transform.position = transform.position;
            if (_dropable == null && onDropEvent == null)
            {
                //this doesn't work
                eventData.pointerDrag.transform.position = transform.position;
            }
        }
    }
}