using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class Codex : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject bookViewerUI;

    public void OnPointerDown(PointerEventData eventData)
    {
        bookViewerUI.SetActive(true);
    }
}
