using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverSprite;
    public float scaleRate = 1.5f;
    private Sprite originalSprite;
    private Vector3 originalScale;
    private Image buttonImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        if (originalSprite == null && buttonImage != null)
            originalSprite = buttonImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null) 
            buttonImage.sprite = hoverSprite;
        //scale up
        transform.localScale = originalScale * scaleRate;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (originalSprite != null) 
            buttonImage.sprite = originalSprite;
        //scale up
        transform.localScale = originalScale;
    }
    void OnDisable()   // Fires when the GameObject is deactivated or scene changes
    {
        if (originalSprite != null && buttonImage != null)
            buttonImage.sprite = originalSprite;
        transform.localScale = originalScale;
    }
}


