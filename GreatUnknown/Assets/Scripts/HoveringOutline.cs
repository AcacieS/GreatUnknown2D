using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoveringOutline : MonoBehaviour
{
    // [SerializeField] private GameObject hoverOutline;
    // Vector3 mousePosition;
    // RaycastHit2D raycastHit2D;
    // Transform prevHoverObject, nextHoverObject;
    // private GameObject currentObject;

    // private void Start()
    // {
    //     currentObject = gameObject;
    // }

    // void Update()
    // {
    //     mousePosition = Mouse.current.position.ReadValue();
    //     Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

    //     prevHoverObject = nextHoverObject;
        
    //     raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

    //     nextHoverObject = (raycastHit2D.collider != null) ? raycastHit2D.collider.transform : null;
        
    //     if (nextHoverObject && nextHoverObject.childCount == 0)
    //         nextHoverObject = nextHoverObject.parent;

    //     if (nextHoverObject) {
    //         if (prevHoverObject && prevHoverObject.gameObject == currentObject)
    //         {
    //             prevHoverObject.transform.GetChild(0).gameObject.SetActive(false);
    //         }
    //         if (nextHoverObject.gameObject == currentObject) {
    //             nextHoverObject.transform.GetChild(0).gameObject.SetActive(true);
    //         }
            
                
    //     } else {
    //         if (prevHoverObject && prevHoverObject.gameObject == currentObject)
    //         {
    //             prevHoverObject.transform.GetChild(0).gameObject.SetActive(false);
    //         } 
    //     }
        
    // }
    [SerializeField] private GameObject hoverOutline; // optional manual assignment
    public bool canShowOutline = true;
    public bool stopShowOutlineForever = false;
    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform prevHoverObject, nextHoverObject;

    private GameObject currentObject;

    private void Start()
    {
        currentObject = gameObject;
    }

    void Update()
    {
        if(stopShowOutlineForever) return;
        mousePosition = Mouse.current.position.ReadValue();
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

    // Example function to enable outline again
    public void EnableOutline()
    {
        canShowOutline = true;
    }
    
}
