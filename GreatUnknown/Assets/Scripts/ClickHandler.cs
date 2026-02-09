using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ClickHandler : MonoBehaviour
{
    [SerializeField] 
    private UnityEvent _clicked;

    private MouseInputProvider _mouse;
    private BoxCollider2D _collider;
    private IClickable _clickable;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _mouse = FindObjectOfType<MouseInputProvider>();
        _mouse.Clicked += MouseOnClicked;

        // Cache interface if implemented on this object
        _clickable = GetComponent<IClickable>();
    }

    private void OnDestroy()
    {
        if (_mouse != null)
            _mouse.Clicked -= MouseOnClicked;
    }

    private void MouseOnClicked()
    {
        if (_collider.bounds.Contains(_mouse.WorldPosition))
        {
            // Call interface method
            _clickable?.OnClick();

            // Also still invoke UnityEvent
            _clicked?.Invoke();
        }
    }
}