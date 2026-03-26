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
    private bool choiceIsMutated = false;
    private bool isGettingIn = false;
    private bool isGettingOut = false;
    private FishSound fishSound;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        clickHandler = GetComponent<ClickHandler>();
        fishSound = GetComponent<FishSound>();
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
            fishSound.PlayRandomWetSound();
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
            SetChoiceIsMutated(false);
            transform.position = new Vector2(InPos.position.x, transform.position.y);
            isInitialPos = true;

            FishManagement.Instance.InitializeNewFish();
            if (stickerObj != null)
            {
                Debug.LogWarning("--Reset sticker get in");
                StickerManagement.Instance.ResetSticker();
                stickerObj = null;
            }
            _collider.enabled = true;
        }
        
        if (Mathf.Abs(transform.position.x - fishPos.position.x) <= 0.2f)
        {
            clickHandler.SetEnableDrag(true);
            isGettingIn = false;
            isInitialPos = false;
        }
        else
        {
            Vector2 target = new Vector2(fishPos.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
        }
        
    }
    public void StickerRemove()
    {
        stickerObj = null;
    }
    private void GetOut()
    {
        clickHandler.SetEnableDrag(false);
        if (Mathf.Abs(transform.position.x - OutPos.position.x) <= 0.1f)
        {
            if (stickerObj != null)
            {
                StickerManagement.Instance.ResetSticker();
                SetChoiceIsMutated(false);
                stickerObj = null;
            }
            isGettingOut = false;
            isGettingIn = true;
            
        }
        else
        {
            Vector2 target = new Vector2(OutPos.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, target,speed*Time.deltaTime);
        }
    }
}
