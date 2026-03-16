using UnityEngine;

public class FishState : MonoBehaviour, IDropable
{
    
    [SerializeField] private Transform InPos;
    [SerializeField] private Transform fishPos;
    [SerializeField] private Transform OutPos;
    [SerializeField] private Sprite stickerOnFish;
    [SerializeField] private float speed = 1f;
    private ClickHandler clickHandler;
    private BoxCollider2D _collider;
    private Fish currentFish;
    private bool choiceIsMutated = false;
    private bool isGettingIn = false;
    private bool isGettingOut = false;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        clickHandler = GetComponent<ClickHandler>();
    }
    public bool GetChoiceIsMutated()
    {
        return choiceIsMutated;
    }
    public void ResetFishGame()
    {
        isGettingIn = true;
        choiceIsMutated = false;
        isInitialPos = false;
    }
    public void SetChoiceIsMutated(bool choiceIsMutated)
    {
        this.choiceIsMutated = choiceIsMutated;
    }
    private GameObject stickerObj = null;
    public void OnDropEvent(GameObject droppedObject)
    {
        if(droppedObject.tag == "sticker")
        {
            Fish currentFish = FishManagement.Instance.GetCurrentFish();
            Vector2 tagPos = currentFish.GetFishType().tagFishPos;

            droppedObject.GetComponent<SpriteRenderer>().sprite = stickerOnFish;
            SetChoiceIsMutated(true);
            // Make it a child of this object
            droppedObject.transform.SetParent(transform);

            // Reset local position so it aligns perfectly
            droppedObject.transform.localPosition = tagPos;
            stickerObj = droppedObject;
        }
    }
    

    private void Update()
    {
        if (isGettingIn)
        {
            GetIn();
        }else if (isGettingOut)
        {
            GetOut();
        }
    }
    public void NextFish()
    {
        if(isGettingIn) return;
        isGettingOut = true;
    }
    private bool isInitialPos = false;

    private void GetIn()
    {
        clickHandler.SetEnableDrag(false);
        if (!isInitialPos)
        {
            transform.position = InPos.position;
            isInitialPos = true;
            //SetChoiceIsMutated(false);
            //stickerObj = null;
            FishManagement.Instance.InitializeNewFish();
            if (stickerObj != null)
            {
                Debug.LogWarning("Reset sticker get in");
                StickerManagement.Instance.ResetSticker();
                stickerObj = null;
                SetChoiceIsMutated(false);
            }
            _collider.enabled = true;
        }

        if (Vector2.Distance(transform.position, fishPos.position) <= 0.1f)
        {
            clickHandler.SetEnableDrag(true);
            isGettingIn = false;
            isInitialPos = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, fishPos.position,speed*Time.deltaTime);
        }
        
    }
    public void StickerRemove()
    {
        stickerObj = null;
    }
    private void GetOut()
    {
        clickHandler.SetEnableDrag(false);
        Vector2 positionWithoutY = transform.position;
        positionWithoutY.y = OutPos.position.y;
        if (Vector2.Distance(positionWithoutY, OutPos.position) <= 0.1f)
        {
            if (stickerObj != null)
            {
                Debug.LogWarning("Reset sticker get out");
                StickerManagement.Instance.ResetSticker();
                stickerObj = null;
                SetChoiceIsMutated(false);
            }
            isGettingOut = false;
            isGettingIn = true;
            
        }
        else
        {
            Vector2 movePos = Vector2.MoveTowards(transform.position, OutPos.position,speed*Time.deltaTime);
            movePos.y = transform.position.y;
            transform.position = movePos;
        }
    }
}
