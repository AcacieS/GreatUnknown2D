using UnityEngine;
using UnityEngine.InputSystem;

public class Dragventory : MonoBehaviour
{
    [Header("Dragventory Screen")]
    [SerializeField] private GameObject dragventoryPanel;
    [Header("Dragventory Colliders and Layers")]
    [SerializeField] private LayerMask dragItemLayer;
    [SerializeField] private Collider2D dragZone;
    [SerializeField] [Range(0, 50)] private float dragSmoothening = 16;

    [Header("Input Action References")]
    [SerializeField] private InputActionReference interaction;
    [SerializeField] private InputActionReference mousePosition;


    // Dragging State
    private Collider2D heldDragItem;
    private Vector3 heldDragItemOffset;

    void OnEnable()
    {
        dragventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Hold on to a a new drag item.
        if (interaction.action.WasPressedThisFrame() && (heldDragItem = GetDragItemUnderMouse()))
        {
            heldDragItemOffset = heldDragItem.transform.position - GetMouseWorldPosition();
        }
        // Release the currently held drag item.
        else if (interaction.action.WasCompletedThisFrame())
        {
            // TODO: Open relevant drag item canvas UI
            heldDragItem = null;
        }
    }

    void FixedUpdate()
    {
        // Move the held drag item.
        if (heldDragItem)
        {
            // TODO: Play animation when drag item is moved to an activation zone.

            // Remember the previous position of the held drag item.
            Vector3 oldPosition = heldDragItem.transform.position;
            // Calculate its new position based on the mouse cursor...
            Vector3 newPosition = GetMouseWorldPosition() + heldDragItemOffset;
            // ...while respecting the camera's boundary...
            newPosition = ClampVector3ToBounds(newPosition, dragZone.bounds);
            // ...and applying a smooth transition effect.
            newPosition = Vector3.Lerp(oldPosition, newPosition, dragSmoothening * Time.deltaTime);
            // Update the position of the held drag item.
            heldDragItem.transform.position = newPosition;
        }
    }

    Vector3 ClampVector3ToBounds(Vector3 vector3, Bounds bounds)
    {
        return new Vector3(
            Mathf.Clamp(vector3.x, bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x),
            Mathf.Clamp(vector3.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y),
            Mathf.Clamp(vector3.z, bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z)
        );
    }

    Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
    }

    Collider2D GetDragItemUnderMouse()
    {
        return Physics2D.OverlapPoint(GetMouseWorldPosition(), dragItemLayer);
    }
}
