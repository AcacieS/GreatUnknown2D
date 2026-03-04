using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance {get; private set;}
    [SerializeField] private FishSession fishSession;
    [SerializeField] private SpriteRenderer daySpriteRenderer;
    [SerializeField] private Sprite[] daySprites;
    [SerializeField] private TypingEffect dayAnimation;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject workplace;
    public static int nbDaysPassed = 0;
    [Header("DEBUG")]
    [SerializeField] private bool isSlidingGameTrue = false;
    [SerializeField] private bool isFishGameTrue = false;
    [Header("Game States")]
    public bool isFishGameFinished = false;
    // TODO: isSliding Game, for now assume is finished;
    public bool isSlidingGameFinished = false;
    [Header("Initialization of Game")]
    [SerializeField] private GameObject workGame;
    [SerializeField] private GameObject fishGame;
    [SerializeField] private GameObject iceSlidingGame;
    [SerializeField] private GameObject Canvas;
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
        workGame.SetActive(true);
        fishGame.SetActive(false);
        iceSlidingGame.SetActive(false);
    }
    public void ResetDay()
    {
        ResetDataDay();
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
    workplace.SetActive(false);
    iceSlidingGame.SetActive(true);
    animator.SetBool("StartingSlidingGame", false);
    }
    private void ResetDataDay()
    {
        isFishGameFinished = false;
        isSlidingGameFinished = false;
        fishSession.ResetSession();
    }
    public void NextDay()
    {
        if(isFishGameFinished && isSlidingGameFinished)
        {
            ResetDataDay();
            nbDaysPassed++;
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
}
