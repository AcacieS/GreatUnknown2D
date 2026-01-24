using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance {get; private set;}
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    void Start()
    {
        //put for onclick of the fish place.
        // FishManagement.Instance.StartFishGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
