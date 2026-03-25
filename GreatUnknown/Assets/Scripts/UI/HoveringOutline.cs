using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoveringOutline : MonoBehaviour, TWTWControls.IPointerActions
{
    [SerializeField] private GameObject hoverOutline; // optional manual assignment

    public bool canShowOutline = true;
    public bool stopShowOutlineForever = false;
    RaycastHit2D raycastHit2D;
    Transform prevHoverObject, nextHoverObject;

    private GameObject currentObject;

    private TWTWControls controlMaps;

    private void Start()
    {
        currentObject = gameObject;
    }

    void UpdateOutline(Vector2 mousePosition)
    {
        if (stopShowOutlineForever) return;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        prevHoverObject = nextHoverObject;

        raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        nextHoverObject = (raycastHit2D.collider != null) ? raycastHit2D.collider.transform : null;

        if (nextHoverObject && nextHoverObject.childCount == 0)
            nextHoverObject = nextHoverObject.parent;

        if (nextHoverObject)
        {
            if (prevHoverObject && prevHoverObject.gameObject == currentObject)
                SetOutline(prevHoverObject, false);

            if (nextHoverObject.gameObject == currentObject)
            {
                SetOutline(nextHoverObject, true);
            }
        }
        else
        {
            if (prevHoverObject && prevHoverObject.gameObject == currentObject)
                SetOutline(prevHoverObject, false);
        }
    }

    void SetOutline(Transform obj, bool state)
    {
        GameObject outline = hoverOutline;
        //Debug.Log("outline"+outline);
        if (outline == null && obj.childCount > 0)
            outline = obj.GetChild(0).gameObject;

        if (outline != null)
            outline.SetActive(state);
    }

    public void DisableOutline()
    {
        stopShowOutlineForever = true;
        canShowOutline = false;
        SetOutline(transform, false);
    }

    public void EnableOutline()
    {
        canShowOutline = true;
    }

    #region Action Bindings for IPointerActions

    void Awake()
    {
        controlMaps = new TWTWControls();
        controlMaps.Pointer.AddCallbacks(this);
    }

    public void OnPoint(InputAction.CallbackContext context)
        => UpdateOutline(context.ReadValue<Vector2>());

    void OnDestroy()
    {
        controlMaps.Dispose();
    }

    void OnEnable()
    {
        controlMaps.Pointer.Enable();
    }

    void OnDisable()
    {
        controlMaps.Pointer.Disable();
    }

    #endregion Action Bindings for IPointerActions
}
