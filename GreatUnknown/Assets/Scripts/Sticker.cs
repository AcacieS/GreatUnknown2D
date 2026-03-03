using UnityEngine;

public class Sticker : MonoBehaviour, IClickable
{
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
            }
        }
        
        //StickerManagement.Instance.ResetSticker();
    }
}
