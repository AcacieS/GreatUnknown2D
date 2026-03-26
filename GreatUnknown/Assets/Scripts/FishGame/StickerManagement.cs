using UnityEngine;

public class StickerManagement : MonoBehaviour
{
    public static StickerManagement Instance {get; private set;}
    public static GameObject sticker;
    [SerializeField] private GameObject stickerColliderPrefab;
    private GameObject fishState;
    public void SetFishState(GameObject newFishState)
    {
        fishState = newFishState;
    }
    public void SetSticker(GameObject newSticker)
    {
        sticker = newSticker;
    }
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    public void ResetSticker()
    {
        SpawnNewStickerCollider();
    }
    public void SpawnNewStickerCollider()
    {
        GameObject newStickerCollider = Instantiate(stickerColliderPrefab,
        transform.position,
        stickerColliderPrefab.transform.rotation,
        transform);
        SoundManager.instance.PlaySound("fishWet" + Random.Range(1, 3));
        Destroy(sticker);
    }

    void OnDisable()
    {
        if(sticker!=null && sticker.transform.parent!=null && sticker.transform.parent.gameObject == fishState) {
            return;
        }
        ResetSticker();
    }
}
