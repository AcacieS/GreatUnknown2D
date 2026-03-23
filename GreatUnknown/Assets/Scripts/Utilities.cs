using UnityEngine;

public class Utilities
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 public static GameObject FindParentWithTag(GameObject child, string tag)
    {
    Transform current = child.transform.parent;
    while (current != null)
    {
        // CompareTag is more performant than transform.tag == tag
        if (current.CompareTag(tag))
        {
            return current.gameObject;
        }
        current = current.parent;
    }
    return null; // No parent with that tag found
    }
}
