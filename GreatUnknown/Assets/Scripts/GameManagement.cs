using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance {get; private set;}
    [SerializeField] private FishSession fishSession;
    [SerializeField] private SpriteRenderer daySpriteRenderer;
    [SerializeField] private Sprite[] daySprites;
    [SerializeField] private TypingEffect dayAnimation;
    [SerializeField] private Animator animator;
    public static int nbDaysPassed = 0;
    [Header("DEBUG")]
    [SerializeField] private bool isSlidingGameTrue = false;
    [SerializeField] private bool isFishGameTrue = false;
    [Header("Game States")]
    public bool isFishGameFinished = false;
    // TODO: isSliding Game, for now assume is finished;
    public bool isSlidingGameFinished = false;
    [SerializeField] private Radio radioScript;
    
    [Header("Initialization of Game")]
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGame;
    [SerializeField] private GameObject iceSlidingGame;
    [SerializeField] private GameObject[] iceSlidingGames;
    [SerializeField] private GameObject Canvas;

    [Header ("Story")]
    [SerializeField] private Light2D emergencyLight = null;
    [Header ("Change Background")]
    [SerializeField] private GameObject backgroundWork = null;
    public Sprite backgroundDay3;
    public Sprite backgroundDay5;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        fishSession.ResetSession();
        OrganizeGame();
    }
    private void OrganizeGame()
    {
        Canvas.SetActive(true);
        workPlace.SetActive(true);
        fishGame.SetActive(false);
        iceSlidingGame.SetActive(false);
    }
    public void ResetDay()
    {
        ResetDataDay();
        fishGame.SetActive(false);
        dayAnimation.WriteText();
        workPlace.SetActive(true);
        FishManagement.Instance.ResetFishGame();
    }
    
    public int GetNbDayPassed()
    {
        return nbDaysPassed;
    }
    public int GetNbDayLeft()
    {
        return daySprites.Length - nbDaysPassed -1;
    }
    public void StartSlidingGame()
    {
        if (GetNbDayLeft() == 0)
        {
            // The player just clicked the computer
            // for the final terminal scene
            // Let's show them what we've got...!
            // after the transition completes, lol.
            animator.SetBool("StartingSlidingGame", true);
            Debug.Log("StartSlidingGame skipped for terminal");
            return;
        }
        if (isFishGameFinished)
        {
            Debug.Log("Fish game artificially started (and it works).");
            //SlidingGameAnimation();
            animator.SetBool("StartingSlidingGame", true);
            
            //MakeNotIceSlidingGame elemnt Dead()
            //Animate Ice Sliding Game.
            
        }
        
    }
    public void OnSlidingTransitionComplete()
    {
        workPlace.SetActive(false);
        animator.SetBool("StartingSlidingGame", false);
        if (GetNbDayLeft() == 0)
        {
            // The player just clicked the computer
            // for the final terminal scene
            // Let's show them what we've got!

            // I dont know what is LastDay!!!!
            // LastDay.Instance.ItsTheFinalCountdown();
            return;
        }
        //iceSlidingGame.SetActive(true);
        iceSlidingGames[nbDaysPassed].SetActive(true);
    }
    private void ResetDataDay()
    {
        isFishGameFinished = false;
        isSlidingGameFinished = false;
        fishSession.ResetSession();
        SpecialEventDay();
    }

    public void ExitSlidingGame()
    {
        iceSlidingGames[nbDaysPassed].SetActive(false);
        workPlace.SetActive(true);

        animator.SetBool("ExitingSlidingGame", true);
    }
    public void OnSlidingExitComplete()
{
    animator.SetBool("ExitingSlidingGame", false);
}


    public void NextDay()
    {
        if(isFishGameFinished && isSlidingGameFinished)
        {
            nbDaysPassed++;
            ResetDataDay();
            daySpriteRenderer.sprite = daySprites[nbDaysPassed];
            if (isSlidingGameTrue)
            {
                isSlidingGameFinished = true;
            }
            if (isFishGameTrue)
            {
                isFishGameFinished = true;
            }
            dayAnimation.WriteText();
        }
    }
    private void SpecialEventDay()
    {
        SpriteRenderer backgroundRend= backgroundWork.GetComponent<SpriteRenderer>();
        switch(nbDaysPassed) 
        {
        case 2: // day 3
            backgroundRend.sprite = backgroundDay3;
            break;
        case 3: //day 4
            // code block
            Debug.Log("Day 4");
            //replace 4th day channel 3. 
            radioScript.ChangeRadioChannel();
            break;
        case 4: //day 5
            // code block
            backgroundRend.sprite = backgroundDay5;
            Debug.Log("Day 5");
            radioScript.ShootingRadioChannel();
            //5th radio starts on ex 30s. no other channel. can close when click on. close it. no channel after.
            break;
        case 5: // day 6
            StartCoroutine(waitEmergencyLight());
            emergencyLight.gameObject.SetActive(true);

            break;
        default:
            Debug.Log("Nothing for now");
            //6th day radio channel is off.
            break;
        }
    }
    public void GetCurrentMusic()
    {
        
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
    public IEnumerator waitEmergencyLight()
    {
        yield return new WaitForSeconds(20);
    }
}
