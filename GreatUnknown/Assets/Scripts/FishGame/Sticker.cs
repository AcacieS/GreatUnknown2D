using UnityEngine;

public class Sticker : MonoBehaviour, IClickable, IDraggable
{
    [SerializeField] private Sprite stickerSprite;
    [SerializeField] private Vector2 colliderStickerOnHand = new Vector2(5f, 2f);
    private Vector3 startPosSticker;
    private ClickHandler clickHandler;
    private bool isBox = true;
    private HoveringOutline hoveringOutline;

    public void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        startPosSticker = transform.position;
        isBox = true;
    }
    public void Start()
    {
        StickerManagement.Instance.SetSticker(gameObject);
        clickHandler = GetComponent<ClickHandler>();
        hoveringOutline = GetComponent<HoveringOutline>();
    }
    public void OnClick()
    {
        if (isBox)
        {
            hoveringOutline.DisableOutline();
            transform.position = clickHandler.StickerMousePos();//startPosSticker;
            isBox = false;
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = colliderStickerOnHand; 
            boxCollider.offset = new Vector2(boxCollider.offset.x, 0);
            GetComponent<SpriteRenderer>().sprite = stickerSprite;
        }
        else
        {
            if(transform.parent != null)
            {
                FishState fishState = transform.parent.GetComponent<FishState>();
                if(fishState != null)
                {
                    fishState.SetChoiceIsMutated(false);
                    transform.SetParent(null);
                    fishState.StickerRemove();
                    GetComponent<SpriteRenderer>().sprite = stickerSprite;
                }
                //that really

            }
        }
        
    }
    public void OnDragEnd()
    {
        if(transform.parent != null && transform.parent.GetComponent<FishState>() != null)
        {
            
        }
        else
        {
            StickerManagement.Instance.ResetSticker();
        }
    }
    
    

}
