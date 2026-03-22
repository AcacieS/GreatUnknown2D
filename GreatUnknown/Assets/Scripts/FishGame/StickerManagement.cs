using UnityEngine;

public class StickerManagement : MonoBehaviour
{
    public static StickerManagement Instance {get; private set;}
    public static GameObject sticker;
    [SerializeField] private GameObject stickerColliderPrefab;
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
        Destroy(sticker);
    }

}
