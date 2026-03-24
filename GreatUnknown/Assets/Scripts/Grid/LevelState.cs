using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

public class LevelState : MonoBehaviour
{
    public bool GameOver { get; private set; }
    public event Action OnGameOver;

    [Header("Limited moves")]
    [SerializeField] private bool useLimitedMoves = false;
    [SerializeField] private int maxMoves = 10;
    [SerializeField] private TextMeshProUGUI movesText;
    
    [Header("Light blink")]
    [SerializeField] private GameObject light;

    public int RemainingMoves { get; private set; }

    private void Awake()
    {
        RemainingMoves = maxMoves;
        RefreshMovesUI();
    }

    public bool CanSpendMove()
    {
        return !useLimitedMoves || RemainingMoves > 0;
    }

    public void SpendMove()
    {
        if (!useLimitedMoves || GameOver)
            return;

        if (RemainingMoves <= 0)
            return;

        RemainingMoves--;
        RefreshMovesUI();

        if (RemainingMoves <= 0)
        {
            SetOutOfMoves();
        }
    }

    private void RefreshMovesUI()
    {
        if (movesText != null)
            movesText.text = "Moves: " + RemainingMoves;
    }

    public void ResetMoves()
    {
        RemainingMoves = maxMoves;
        RefreshMovesUI();
    }

    public void IndicationOfRestart()
    {
        BlinkText lightBlinky;
        if(light==null) return; 
        lightBlinky = light.GetComponent<BlinkText>();
        if (lightBlinky == null) Debug.Log("lightBlinkythereisnone");

    }

    public void SetOutOfMoves()
    {
        
    }

    public void SetGameOver()
    {
        if (GameOver) return;   // prevent double trigger

        if (GameManagement.Instance != null)
        {
            GameManagement.Instance.MarkSlidingGameFinished();
            Debug.Log("GameFisnishedOnGameMangement");
        }

        GameOver = true;
        Debug.Log("GOAL REACHED — GAME OVER");
        OnGameOver?.Invoke();
    }
}