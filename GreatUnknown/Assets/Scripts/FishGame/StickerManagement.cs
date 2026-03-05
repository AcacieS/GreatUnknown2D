using UnityEngine;

public class StickerManagement : MonoBehaviour, IClickable
{
    public static StickerManagement Instance {get; private set;}
    public static bool hasSticker = false;
    [SerializeField] private GameObject stickerPrefab;
    [SerializeField] public GameObject stickerSpawnPlace;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ResetSticker()
    {
        hasSticker = false;
    }
    
    public void OnClick()
    {
        if(hasSticker) return;
        hasSticker = true;
        GameObject newStickerSpawn = Instantiate(stickerPrefab,
        stickerSpawnPlace.transform.position,
        stickerSpawnPlace.transform.rotation);
    }
}
