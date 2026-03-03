using UnityEngine;

public class Sticker : MonoBehaviour, IClickable
{
    private Sprite initialSticker;
    public void Start()
    {
        initialSticker = GetComponent<SpriteRenderer>().sprite;
    }
    public void OnClick()
    {
        if(transform.parent != null)
        {
            FishState fishState = transform.parent.GetComponent<FishState>();
            if(fishState != null)
            {
                fishState.SetChoiceIsMutated(false);
                transform.SetParent(null);
                fishState.StickerRemove();
                GetComponent<SpriteRenderer>().sprite = initialSticker;
            }
        }
        
        //StickerManagement.Instance.ResetSticker();
    }
}
