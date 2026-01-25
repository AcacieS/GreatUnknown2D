using UnityEngine;

public class ClickTestReceiver : MonoBehaviour
{
    public void LogClickedObject(GameObject clicked)
    {
        Debug.Log($"Clicked object: {clicked.name}", clicked);
    }
}
