using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoveringOutline : MonoBehaviour
{
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
        mousePosition = Mouse.current.position.ReadValue();
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        prevHoverObject = nextHoverObject;
        
        raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

        nextHoverObject = (raycastHit2D.collider != null) ? raycastHit2D.collider.transform : null;
        
        if (nextHoverObject && nextHoverObject.childCount == 0)
            nextHoverObject = nextHoverObject.parent;

        if (nextHoverObject) {
            if (prevHoverObject && prevHoverObject.gameObject == currentObject)
            {
                prevHoverObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (nextHoverObject.gameObject == currentObject) {
                nextHoverObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            
                
        } else {
            if (prevHoverObject && prevHoverObject.gameObject == currentObject)
            {
                prevHoverObject.transform.GetChild(0).gameObject.SetActive(false);
            } 
        }
        
    }
}
