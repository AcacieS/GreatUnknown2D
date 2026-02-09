using TMPro;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance {get; private set;}
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private FishSession fishSession;
    public static int nbDaysPassed = 0;
    [Header("DEBUG")]
    [SerializeField] private bool isSlidingGameTrue = false;
    [SerializeField] private bool isFishGameTrue = false;
    [Header("Game States")]
    public bool isFishGameFinished = false;
    // TODO: isSliding Game, for now assume is finished;
    public bool isSlidingGameFinished = false;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        fishSession.ResetSession();
        
    }
    public int GetNbDayPassed()
    {
        return nbDaysPassed;
    }
    
    public void NextDay()
    {
        if(isFishGameFinished && isSlidingGameFinished)
        {
            
            isFishGameFinished = false;
            isSlidingGameFinished = false;
            nbDaysPassed++;
            fishSession.ResetSession();
            dayText.text = "Day " + nbDaysPassed.ToString();
            if (isSlidingGameTrue)
            {
                isSlidingGameFinished = true;
            }
            if (isFishGameTrue)
            {
                isFishGameFinished = true;
            }
        }
    }
    void Start()
    {
        if (isSlidingGameTrue)
        {
            isSlidingGameFinished = true;
        }
        if (isFishGameTrue)
        {
            isFishGameFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
