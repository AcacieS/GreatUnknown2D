using UnityEngine;

public class FishState : MonoBehaviour, IDropable
{
    private bool choiceIsMutated = false;
    private bool isGettingIn = false;
    private bool isGettingOut = false;
    [SerializeField] private Transform InPos;
    [SerializeField] private Transform fishPos;
    [SerializeField] private Transform OutPos;
    [SerializeField] private Sprite stickerOnFish;
    [SerializeField] private float speed = 1f;

    private void Start()
    {
        isGettingIn = true;
    }
    public bool GetChoiceIsMutated()
    {
        return choiceIsMutated;
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
            droppedObject.GetComponent<SpriteRenderer>().sprite = stickerOnFish;
            SetChoiceIsMutated(true);
            // Make it a child of this object
            droppedObject.transform.SetParent(transform);

            // Reset local position so it aligns perfectly
            droppedObject.transform.localPosition = Vector3.zero;
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
        if (!isInitialPos)
        {
            transform.position = InPos.position;
            isInitialPos = true;
            FishManagement.Instance.InitializeNewFish();
        }

        if (Vector2.Distance(transform.position, fishPos.position) <= 0.1f)
        {
            isGettingIn = false;
            isInitialPos = false;
        }
        else
        {
            Debug.Log("Moving In");
            transform.position = Vector2.MoveTowards(transform.position, fishPos.position,speed*Time.deltaTime);
        }
        
    }
    public void StickerRemove()
    {
        stickerObj = null;
    }
    private void GetOut()
    {
        Vector2 positionWithoutY = transform.position;
        positionWithoutY.y = OutPos.position.y;
        if (Vector2.Distance(positionWithoutY, OutPos.position) <= 0.1f)
        {
            if (stickerObj != null)
            {
                StickerManagement.Instance.ResetSticker();
                Destroy(stickerObj);
                stickerObj = null;
                SetChoiceIsMutated(false);
            }
            Debug.Log("Stop");
            isGettingOut = false;
            isGettingIn = true;
            
        }
        else
        {
            Debug.Log("Moving Out");
            Vector2 movePos = Vector2.MoveTowards(transform.position, OutPos.position,speed*Time.deltaTime);
            movePos.y = transform.position.y;
            transform.position = movePos;
        }
    }
}
