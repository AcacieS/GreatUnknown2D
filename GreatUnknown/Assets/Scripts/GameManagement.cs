using System.Collections;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [SerializeField] private FishSession fishSession;
    [SerializeField] private SpriteRenderer daySpriteRenderer;
    [SerializeField] private Sprite[] daySprites;
    [SerializeField] private TypingEffect dayAnimation;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private BGMusic bgSource;
    public static int nbDaysPassed = 0;

    [Header("DEBUG")]
    [SerializeField] private bool isSlidingGameTrue = false;
    [SerializeField] private bool isFishGameTrue = false;
    [SerializeField] private bool activateSaveSystem = true;
    
    
    public event System.Action<bool> OnFishGameFinishedChanged;
    
    private bool _isFishGameFinished = false;
    [Header("Game States")]
    public bool IsFishGameFinished
    {
        get { return _isFishGameFinished; }
        set
        {
            if (_isFishGameFinished != value)
            {
                _isFishGameFinished = value;
                OnFishGameFinishedChanged?.Invoke(value);
            }
        }
    }
    

    public bool isSlidingGameFinished = false;
    public bool isDayEnding = false;

    [Header("Day Ending")]
    [SerializeField] private float delayBeforeFade = 1.5f;

    [SerializeField] private PortholeSwitcher portholeScript;
    [SerializeField] private Radio radioScript;
    

    [Header("Initialization of Game")]
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGame;
    [SerializeField] private GameObject fishOnTray;

    [SerializeField] private GameObject iceSlidingCanvas;
    [SerializeField] private GameObject iceSlidingExitUiRoot; // parent of the exit button

    [SerializeField] private IceSlidingGameSwitcher iceSlidingGameSwitcher;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject firstDayStoryCanvas;
    [SerializeField] private LastDay lastDayMB;

    [Header("Story")]
    [SerializeField] private FaxMachine faxMachine;

    [Header("Change Background")]
    [SerializeField] private SpriteRenderer backgroundRend;
    public Sprite backgroundDay2;
    public Sprite backgroundDay3;
    public Sprite backgroundDay5;

    [Header("Computer")]
    [SerializeField] private Animator computer;
    [SerializeField] private float waitTimeComputerOpen;
    [SerializeField] private float faxMachineWaitSeconds = 3f;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Check references and warn if any are missing from the Inspector.
        if (Canvas == null) Ext.WarnRef("Canvas", this);
        if (workPlace == null) Ext.WarnRef("workPlace", this);
        if (fishGame == null) Ext.WarnRef("fishGame", this);
        if (firstDayStoryCanvas == null) Ext.WarnRef("firstDayCanvas", this);

        // For Load system
        if (activateSaveSystem)
        {
            nbDaysPassed = SaveSystem.LoadProgress();
        }

        fishSession.ResetSession();
        OrganizeGame();
        OnFishGameFinishedChanged += HandleFishFinished;
    }

    void Start()
    {
        if (isSlidingGameTrue)
        {
            isSlidingGameFinished = true;
        }

        if (isFishGameTrue)
        {
            IsFishGameFinished = true;
        }
    }

    private void OrganizeGame()
    {
        // Use a proper null-check (not ?.)
        // see: https://unity.com/blog/engine-platform/custom-operator-should-we-keep-it
        if (Canvas != null) Canvas.SetActive(true);
        if (workPlace != null) workPlace.SetActive(true);
        if (fishGame != null) fishGame.SetActive(false);
        if (nbDaysPassed==0)
        {
            if (firstDayStoryCanvas != null) firstDayStoryCanvas.SetActive(true);
        }

        if (iceSlidingCanvas != null)
            iceSlidingCanvas.SetActive(false);

        if (iceSlidingGameSwitcher != null)
            iceSlidingGameSwitcher.DeactivateAll();
        if (fishOnTray != null) fishOnTray.SetActive(true);
    }

    public void ResetDay()
    {
        ResetDataDay();

        if (fishGame != null)
            fishGame.SetActive(false);

        dayAnimation.WriteText();

        if (workPlace != null)
            workPlace.SetActive(true);
        FishManagement.Instance.ResetFishGame();
        IsFishGameFinished = false;
    }

    public int GetNbDayPassed()
    {
        return nbDaysPassed;
    }

    public int GetNbDayLeft()
    {
        return daySprites.Length - nbDaysPassed - 1;
    }

    public void StartSlidingGame()
    {
        if (!IsFishGameFinished) return;
        if (GetNbDayPassed() == 5) return;
        if (animator == null) return;

        Debug.Log("Starting sliding game transition.");
        animator.SetBool("StartingSlidingGame", true);
    }

    // Called by SlidingTransitionRelay on the camera animator
    public void OnSlidingTransitionComplete()
    {
        if (animator != null)
            animator.SetBool("StartingSlidingGame", false);

        StartCoroutine(OpenSlidingGameRoutine());
    }

    private IEnumerator OpenSlidingGameRoutine()
    {
        if (workPlace != null)
            workPlace.SetActive(false);
    
        if (iceSlidingCanvas != null)
            iceSlidingCanvas.SetActive(true);

        // Let the canvas fully enable before activating the day-specific sliding game
        yield return null;

        if (iceSlidingGameSwitcher != null)
            iceSlidingGameSwitcher.ActivateForDay(nbDaysPassed);
    }

    public void ExitSlidingGame()
    {
        // Disable UI first so it cannot keep interfering
        if (iceSlidingExitUiRoot != null)
            iceSlidingExitUiRoot.SetActive(false);

        // Then disable the sliding canvas
        if (iceSlidingCanvas != null)
            iceSlidingCanvas.SetActive(false);

        // Then deactivate the active day-specific sliding game
        if (iceSlidingGameSwitcher != null)
            iceSlidingGameSwitcher.DeactivateCurrent();

        if (workPlace != null) 
            workPlace.SetActive(true);

        if (animator != null)
            animator.SetBool("ExitingSlidingGame", true);
    }
    // Called by SlidingTransitionRelay on the camera animator
    public void OnSlidingExitComplete()
    {
        if (animator != null)
            animator.SetBool("ExitingSlidingGame", false);

        if (workPlace != null)
            workPlace.SetActive(true);

        TryStartDayEnding();
    }

    private void ResetDataDay()
    {
        StopAllCoroutines();

        if (radioScript != null)
            radioScript.StopAllCoroutines();

        IsFishGameFinished = false;
        isSlidingGameFinished = false;
        isDayEnding = false;

        fishSession.ResetSession();
        portholeScript.OnDaySwitch(nbDaysPassed);
        if (computer != null)
        {
            computer.SetBool("Open", false);
        }
        
        if (fishOnTray != null) fishOnTray.SetActive(true);

        //SpecialEventDay();
        
    }

    public void MarkSlidingGameFinished()
    {
        if (isSlidingGameFinished) return;

        isSlidingGameFinished = true;
    }
    [ContextMenu("Ending Day")]
    private void TryStartDayEnding()
    {
        if (isDayEnding) return;
        if (!IsFishGameFinished) return;
        if (!isSlidingGameFinished) return;
        if (workPlace == null || !workPlace.activeSelf) return;
        if(computer!=null) {
            computer.SetBool("Open", false);
        }
        isDayEnding = true;
        StartCoroutine(DayEndingRoutine());
    }

    private IEnumerator DayEndingRoutine()
    {
        Debug.Log("Day ending routine started.");

        yield return new WaitForSeconds(delayBeforeFade);

        Debug.Log("Fade Out");
        if (fadeOut != null)
            fadeOut.SetActive(true);//SetTrigger("FadeOutDay");
    }

    public void OnDayFadeComplete()
    {
        NextDay();
    }

    public void NextDay()
    {
        nbDaysPassed++;
        ResetDataDay();

        // For Load system
        SaveSystem.SaveProgress(nbDaysPassed);

        if (nbDaysPassed < daySprites.Length)
            daySpriteRenderer.sprite = daySprites[nbDaysPassed];

        dayAnimation.WriteText();
    }

    public void SpecialEventDay()
    {
        switch (nbDaysPassed)
        {
            case 0:
                StartCoroutine(WaitFaxMachineMorningDay("day1_morning"));
                bgSource?.PlayNewBGMusic(BGMusicType.engine);
                break;
            case 1:
                StartCoroutine(WaitFaxMachineMorningDay("day2_morning"));
                if (backgroundRend != null)
                    backgroundRend.sprite = backgroundDay2;
                break;

            case 2: // day 3
                StartCoroutine(WaitFaxMachineMorningDay("day3_morning"));
                // faxMachine?.NewFaxMessage("day3_morning");
                bgSource?.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.creaky);
                if (backgroundRend != null)
                    backgroundRend.sprite = backgroundDay3;
                if (IsFishGameFinished)
                    faxMachine?.NewFaxMessage("day3_midday");
                break;

            case 3: // day 4
                StartCoroutine(WaitFaxMachineMorningDay("day4_morning"));
                faxMachine?.NewFaxMessage("day4_morning");
                radioScript?.ChangeRadioChannel();
                break;

            case 4: // day 5
                StartCoroutine(WaitFaxMachineMorningDay("day5_morning"));
                if (backgroundRend != null)
                    backgroundRend.sprite = backgroundDay5;
                radioScript?.ShootingRadioChannel();
                break;
            case 5: // day 6
                // LastDay handles emergency lights and sound
                bgSource?.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.anomaly);
                if (lastDayMB != null)
                    lastDayMB.enabled = true;
                break;

            default:
                Debug.Log("Nothing for now");
                break;
        }
    }
    private IEnumerator WaitFaxMachineMorningDay(string faxMessage)
    {
        yield return new WaitForSeconds(faxMachineWaitSeconds);
        faxMachine?.NewFaxMessage(faxMessage);
    }
    
    void HandleFishFinished(bool finished)
    {
        if (finished)
        {
            Debug.Log("Fish game is finished!");
            fishOnTray.SetActive(false);
            StartCoroutine("ComputerOpen");
        }
    }
    
    
    private IEnumerator ComputerOpen()
    {
        yield return new WaitForSeconds(waitTimeComputerOpen);
        Debug.Log("Computer opened");
        if (computer != null)
        {
            computer.SetBool("Open", true);
        }
    }

}