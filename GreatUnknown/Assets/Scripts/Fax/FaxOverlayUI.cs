using UnityEngine;

public class FaxOverlayUI : MonoBehaviour
{
    [SerializeField] private GameObject overlayRoot;

    public void Open()
    {
        overlayRoot.SetActive(true);
    }

    public void Close()
    {
        overlayRoot.SetActive(false);
    }
}