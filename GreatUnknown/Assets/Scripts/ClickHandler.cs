using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _clicked;
    private MouseInputProvider _mouse;
    private BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _mouse = FindObjectOfType<MouseInputProvider>();
        _mouse.Clicked += MouseOnClicked;
    }
    private void MouseOnClicked()
    {
        if (_collider.bounds.Contains(_mouse.WorldPosition))
        {
            _clicked?.Invoke();
        }
    }
}
