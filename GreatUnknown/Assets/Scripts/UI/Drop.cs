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
        if(eventData.pointerDrag != null)
        {
            _dropable?.OnDropEvent(eventData.pointerDrag);
            onDropEvent?.Invoke();
        }
    }
}