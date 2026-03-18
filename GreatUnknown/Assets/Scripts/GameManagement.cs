using System.Collections;
using NUnit.Framework;
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
    public bool isSlidingGameFinished = false;
    public bool isDayEnding = false;

    [Header("Day Ending")]
    [SerializeField] private float delayBeforeFade = 1.5f;
    private Coroutine dayEndingCoroutine;

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
        if (isFishGameFinished && GetNbDayPassed() != 5)
        {
            Debug.Log("Fish game artificially started (and it works).");
            animator.SetBool("StartingSlidingGame", true);
        }
    }

    public void OnSlidingTransitionComplete()
    {
        workPlace.SetActive(false);
        animator.SetBool("StartingSlidingGame", false);
        iceSlidingGames[nbDaysPassed].SetActive(true);
    }

    private void ResetDataDay()
    {
        isFishGameFinished = false;
        isSlidingGameFinished = false;
        isDayEnding = false;

        if (dayEndingCoroutine != null)
        {
            StopCoroutine(dayEndingCoroutine);
            dayEndingCoroutine = null;
        }

        fishSession.ResetSession();
        SpecialEventDay();
    }

    public void ExitSlidingGame()
    {
        iceSlidingGames[nbDaysPassed].SetActive(false);
        workPlace.SetActive(true);

        animator.SetBool("ExitingSlidingGame", true);

        TryStartDayEnding();
    }

    public void OnSlidingExitComplete()
    {
        animator.SetBool("ExitingSlidingGame", false);
    }

    public void MarkSlidingGameFinished()
    {
        if (isSlidingGameFinished) return;

        isSlidingGameFinished = true;
        TryStartDayEnding();
    }

    private void TryStartDayEnding()
{
    if (isDayEnding) return;

    if (!isFishGameFinished) return;
    if (!isSlidingGameFinished) return;
    if (workPlace == null || !workPlace.activeSelf) return;

    isDayEnding = true;
    dayEndingCoroutine = StartCoroutine(DayEndingRoutine());
}

    private IEnumerator DayEndingRoutine()
    {
        Debug.Log("Day ending routine started.");

        yield return new WaitForSeconds(delayBeforeFade);

        animator.SetTrigger("FadeOutDay");
    }

    public void OnDayFadeComplete()
    {
        NextDay();
    }

    [ContextMenu("Skip to next day")]
    public void SkipToNextDay()
    {
        isFishGameFinished = true;
        isSlidingGameFinished = true;
        NextDay();
    }

public void NextDay()
{
    nbDaysPassed++;
    ResetDataDay();

    if (nbDaysPassed < daySprites.Length)
    {
        daySpriteRenderer.sprite = daySprites[nbDaysPassed];
    }

    dayAnimation.WriteText();
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
            Debug.Log("Day 4");
            radioScript.ChangeRadioChannel();
            break;
        case 4: //day 5
            backgroundRend.sprite = backgroundDay5;
            Debug.Log("Day 5");
            radioScript.ShootingRadioChannel();
            break;
        case 5: // day 6
            StartCoroutine(waitEmergencyLight());
            emergencyLight.gameObject.SetActive(true);
            LastDay.Instance.gameObject.SetActive(true);
            break;
        default:
            Debug.Log("Nothing for now");
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

    void Update()
    {
        
    }

    public IEnumerator waitEmergencyLight()
    {
        yield return new WaitForSeconds(20);
    }
}