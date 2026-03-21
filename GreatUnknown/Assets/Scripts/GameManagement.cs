using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [SerializeField] private FishSession fishSession;
    [SerializeField] private SpriteRenderer daySpriteRenderer;
    [SerializeField] private Sprite[] daySprites;
    [SerializeField] private TypingEffect dayAnimation;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource bgSource;
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

    [SerializeField] private PortholeSwitcher portholeScript;
    [SerializeField] private Radio radioScript;

    [Header("Initialization of Game")]
    [SerializeField] private GameObject workPlace;
    [SerializeField] private GameObject fishGame;

    [SerializeField] private GameObject iceSlidingCanvas;
    [SerializeField] private IceSlidingGameSwitcher iceSlidingGameSwitcher;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject firstDayCanvas;
    [SerializeField] private LastDay lastDayMB;

    [Header("Story")]
    [SerializeField] private FaxMachine faxMachine;

    [Header("Change Background")]
    [SerializeField] private SpriteRenderer backgroundRend;
    public Sprite backgroundDay3;
    public Sprite backgroundDay5;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        fishSession.ResetSession();
        OrganizeGame();
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

    private void OrganizeGame()
    {
        Canvas?.SetActive(true);
        workPlace?.SetActive(true);
        fishGame?.SetActive(false);
        firstDayCanvas?.SetActive(true);

        if (iceSlidingCanvas != null)
            iceSlidingCanvas.SetActive(false);

        if (iceSlidingGameSwitcher != null)
            iceSlidingGameSwitcher.DeactivateAll();
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
        if (!isFishGameFinished) return;
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
        if (iceSlidingGameSwitcher != null)
            iceSlidingGameSwitcher.DeactivateCurrent();

        if (animator != null)
            animator.SetBool("ExitingSlidingGame", true);
    }

    // Called by SlidingTransitionRelay on the camera animator
    public void OnSlidingExitComplete()
    {
        if (animator != null)
            animator.SetBool("ExitingSlidingGame", false);

        if (iceSlidingCanvas != null)
            iceSlidingCanvas.SetActive(false);

        if (workPlace != null)
            workPlace.SetActive(true);

        TryStartDayEnding();
    }

    private void ResetDataDay()
    {
        StopAllCoroutines();

        if (radioScript != null)
            radioScript.StopAllCoroutines();

        isFishGameFinished = false;
        isSlidingGameFinished = false;
        isDayEnding = false;

        fishSession.ResetSession();
        portholeScript.OnDaySwitch(nbDaysPassed);
        SpecialEventDay();
    }

    public void MarkSlidingGameFinished()
    {
        if (isSlidingGameFinished) return;

        isSlidingGameFinished = true;
    }

    private void TryStartDayEnding()
    {
        if (isDayEnding) return;
        if (!isFishGameFinished) return;
        if (!isSlidingGameFinished) return;
        if (workPlace == null || !workPlace.activeSelf) return;

        isDayEnding = true;
        StartCoroutine(DayEndingRoutine());
    }

    private IEnumerator DayEndingRoutine()
    {
        Debug.Log("Day ending routine started.");

        yield return new WaitForSeconds(delayBeforeFade);

        if (animator != null)
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
            daySpriteRenderer.sprite = daySprites[nbDaysPassed];

        dayAnimation.WriteText();
    }
    
    public void SpecialEventDay()
    {
        switch (nbDaysPassed)
        {
            case 0:
                faxMachine?.NewFaxMessage("day1_morning");
                bgSource?.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.engine);
                break;
            case 1:
                faxMachine?.NewFaxMessage("day2_morning");
                break;

            case 2: // day 3
                faxMachine?.NewFaxMessage("day3_morning");
                bgSource?.GetComponent<BGMusic>().PlayNewBGMusic(BGMusicType.creaky);
                if (backgroundRend != null)
                    backgroundRend.sprite = backgroundDay3;
                break;

            case 3: // day 4
                faxMachine?.NewFaxMessage("day4_morning");
                radioScript?.ChangeRadioChannel();
                break;

            case 4: // day 5
                faxMachine?.NewFaxMessage("day5_morning");
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
}