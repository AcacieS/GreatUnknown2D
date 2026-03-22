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
    [SerializeField] private bool RestrictedToAxisX = false;
    [SerializeField] private bool RestrictedToAxisY = false;
    [Header("Events")]
    [FormerlySerializedAs("_clicked")]
    public UnityEvent onClick;
    
    private IClickable _clickable;
    private BoxCollider2D _collider;
    private IDraggable _draggable;

    public void Start()
    {
        _collider.enabled = true; 
    }

    private void Awake()
    {
        _camera = Camera.main;
        _clickable = GetComponent<IClickable>();
        _collider = GetComponent<BoxCollider2D>();
        _draggable = GetComponent<IDraggable>();
    }
    Vector3 mouseWorld;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (enableDrag)
        {
            mouseWorld = _camera.ScreenToWorldPoint(eventData.position);
            mouseWorld.z = 0f;
            _offset = transform.position - mouseWorld;
        }
        onClick?.Invoke();
        _clickable?.OnClick();
    }
    public Vector3 StickerMousePos()
    {
        _offset = Vector3.zero;
        return mouseWorld;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        mouseWorld = _camera.ScreenToWorldPoint(eventData.position);
        mouseWorld.z = 0f;
        if(RestrictedToAxisX)
        {
            transform.position = new Vector3(mouseWorld.x + _offset.x, transform.position.y, transform.position.z);
        }else if(RestrictedToAxisY)
        {
            transform.position = new Vector3(transform.position.x, mouseWorld.y + _offset.y, transform.position.z);
        }
        else
        {
            transform.position = mouseWorld + _offset;
        }
    }
    public void SetEnableDrag(bool enable)
    {
        enableDrag = enable;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        _collider.enabled = false;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!enableDrag) return;
        _collider.enabled = true;
        _draggable?.OnDragEnd();
    }
    public void OnDrop(PointerEventData eventData)
    {
        
    }
    
}
