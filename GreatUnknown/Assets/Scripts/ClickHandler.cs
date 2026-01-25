using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ClickHandler : MonoBehaviour
{
    [SerializeField]
    private GameObjectEvent _clicked;

    private MouseInputProvider _mouse;
    private BoxCollider2D _collider;
    private IClickable _clickable;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _mouse = FindObjectOfType<MouseInputProvider>();
        if (_mouse != null)
            _mouse.Clicked += MouseOnClicked;

        _clickable = GetComponent<IClickable>();
    }

    private void OnDestroy()
    {
        if (_mouse != null)
            _mouse.Clicked -= MouseOnClicked;
    }

    private void MouseOnClicked()
    {
        if (_mouse == null) return;

        if (_collider.bounds.Contains(_mouse.WorldPosition))
        {
            // Code-level behavior (optional)
            _clickable?.OnClick();
            Debug.Log($"ClickHandler fired on {gameObject.name}", gameObject);

            // Inspector-wired behavior, with context
            _clicked?.Invoke(gameObject);
            Debug.Log($"ClickHandler fired on {gameObject.name}", gameObject);
        }
    }
}
