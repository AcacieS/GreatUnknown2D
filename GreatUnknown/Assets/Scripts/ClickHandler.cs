using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]

public class ClickHandler: MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Camera _camera;
    private Vector3 _offset;

    [Header("Modes")]
    [SerializeField] private bool enableDrag = false;
    [Header("Events")]
    [FormerlySerializedAs("_clicked")]
    [SerializeField] private UnityEvent onClick;
    private IClickable _clickable;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _camera = Camera.main;
        _clickable = GetComponent<IClickable>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        if (enableDrag)
        {
            Vector3 mouseWorld = _camera.ScreenToWorldPoint(eventData.position);
            mouseWorld.z = 0f;
            _offset = transform.position - mouseWorld;
        }
        onClick?.Invoke();
        _clickable?.OnClick();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        Debug.Log("Dragging");
        Vector3 mouseWorld = _camera.ScreenToWorldPoint(eventData.position);
        mouseWorld.z = 0f;

        transform.position = mouseWorld + _offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        Debug.Log("Begin Drag");
        _collider.enabled = false;  
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        Debug.Log("End Drag");
        _collider.enabled = true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        
    }
    
}
