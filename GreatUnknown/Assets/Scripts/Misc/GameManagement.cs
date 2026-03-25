using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    // While we decide whether or not to support options, force 1920x1080.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ForceScreenTo16by9()
    {
        foreach (var res in Screen.resolutions)
        {
            if (res.width == 1920 && res.height == 1080)
            {
                Screen.SetResolution(res.width, res.height, true);
            }
        }
        Screen.SetResolution(1920, 1080, true);
    }

    [SerializeField] private FishSession fishSession;
    [SerializeField] private SpriteRenderer daySpriteRenderer;
    [SerializeField] private Sprite[] daySprites;
    [SerializeField] private TypingEffect dayAnimation;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject lightManagement;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private BGMusic bgSource;
    [SerializeField] private Radio radio;
    public static int nbDaysPassed = 0;
    
    
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

    [Header("Story")]
    [SerializeField] private FaxMachine faxMachine;
    [SerializeField] private LastDay lastDayMB;

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
        if (fishSession            == null) Ext.WarnRef("fishSession", this);
        if (daySpriteRenderer      == null) Ext.WarnRef("daySpriteRenderer", this);
        if (daySprites             == null) Ext.WarnRef("daySprites", this);
        if (dayAnimation           == null) Ext.WarnRef("dayAnimation", this);
        if (animator               == null) Ext.WarnRef("animator", this);
        if (lightManagement        == null) Ext.WarnRef("lightManagement", this);
        if (fadeOut                == null) Ext.WarnRef("fadeOut", this);
        if (bgSource               == null) Ext.WarnRef("bgSource", this);
        if (portholeScript         == null) Ext.WarnRef("portholeScript", this);
        if (radioScript            == null) Ext.WarnRef("radioScript", this);
        if (workPlace              == null) Ext.WarnRef("workPlace", this);
        if (fishGame               == null) Ext.WarnRef("fishGame", this);
        if (fishOnTray             == null) Ext.WarnRef("fishOnTray", this);
        if (iceSlidingCanvas       == null) Ext.WarnRef("iceSlidingCanvas", this);
        if (iceSlidingExitUiRoot   == null) Ext.WarnRef("iceSlidingExitUiRoot", this);
        if (iceSlidingGameSwitcher == null) Ext.WarnRef("iceSlidingGameSwitcher", this);
        if (Canvas                 == null) Ext.WarnRef("Canvas", this);
        if (firstDayStoryCanvas    == null) Ext.WarnRef("firstDayStoryCanvas", this);
        if (lastDayMB              == null) Ext.WarnRef("lastDayMB", this);
        if (faxMachine             == null) Ext.WarnRef("faxMachine", this);
        if (backgroundRend         == null) Ext.WarnRef("backgroundRend", this);
        if (backgroundDay2         == null) Ext.WarnRef("backgroundDay2", this);
        if (backgroundDay3         == null) Ext.WarnRef("backgroundDay3", this);
        if (backgroundDay5         == null) Ext.WarnRef("backgroundDay5", this);
        if (computer               == null) Ext.WarnRef("computer", this);

        fishSession.ResetSession();
        OrganizeGame();
        OnFishGameFinishedChanged += HandleFishFinished;
    }

    [Header("DEBUG")]
    [FormerlySerializedAs("isSlidingGameTrue")]
    [SerializeField] private bool setSlidingGameFinishedTrueOnStart = false;
    [FormerlySerializedAs("isFishGameTrue")]
    [SerializeField] private bool setFishGameFinishedTrueOnStart = false;

    void Start()
    {
        if (setSlidingGameFinishedTrueOnStart)
        {
            isSlidingGameFinished = true;
        }

        if (setFishGameFinishedTrueOnStart)
        {
            IsFishGameFinished = true;
        }

        if (nbDaysPassed != 0) ResetDay();
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
        if (faxMachine != null)
            faxMachine.ClearAllFaxMessages();

        ResetDataDay();

        if (fishGame != null)
            fishGame.SetActive(false);

        dayAnimation.WriteText();

        if (workPlace != null)
            workPlace.SetActive(true);
        FishManagement.Instance.ResetFishGame();
        IsFishGameFinished = false;
        radio.Reset();
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
        if(isSlidingGameFinished) return;
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

        if (lightManagement != null)
            lightManagement.SetActive(false);

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

    /** Called by SlidingTransitionRelay on the camera animator */
    public void OnSlidingExitComplete()
    {
        if (animator != null)
            animator.SetBool("ExitingSlidingGame", false); 

        if (workPlace != null)
            workPlace.SetActive(true);
        
        if (lightManagement != null)
            lightManagement.SetActive(true);

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
        radio?.Save();
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
            Debug.Log("Closed down the computer (was that supposed to happen?)");
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
        StartCoroutine(WaitFaxMachineMorningDay());
        switch (nbDaysPassed)
        {
            case 0:
                if (bgSource != null) bgSource.PlayNewBGMusic(BGMusicType.engine);
                break;
            case 1:
                if (backgroundRend != null) backgroundRend.sprite = backgroundDay2;
                break;

            case 2: // day 3
                if (bgSource != null) bgSource.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.creaky);
                if (backgroundRend != null) backgroundRend.sprite = backgroundDay3;
                break;

            case 3: // day 4
                if (radioScript != null) radioScript.ChangeRadioChannel();
                break;

            case 4: // day 5
                if (backgroundRend != null) backgroundRend.sprite = backgroundDay5;
                if (radioScript != null) radioScript.ShootingRadioChannel();
                break;
            case 5: // day 6
                if (bgSource != null) bgSource.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.anomaly);
                if (lastDayMB != null) lastDayMB.gameObject.SetActive(true);
                IsFishGameFinished = true;
                isSlidingGameFinished = true;
                break;

            default:
                Debug.Log("Nothing for now");
                break;
        }
    }

    private IEnumerator WaitFaxMachineMorningDay()
    {
        yield return new WaitForSeconds(faxMachineWaitSeconds);
        faxMachine.SendStartingFaxMessages(nbDaysPassed);
    }
    
    void HandleFishFinished(bool finished)
    {
        if (finished)
        {
            Debug.Log("Fish game is finished!");
            fishOnTray.SetActive(false);

            if(nbDaysPassed==5) return;
            StartCoroutine("ComputerOpen");
            if(nbDaysPassed == 2 && faxMachine != null)
            {
                faxMachine.NewFaxMessage("day3_midday");
            }
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
