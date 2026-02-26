using UnityEngine;
using System;

public class LevelState : MonoBehaviour
{
    public bool GameOver { get; private set; }

    public event Action OnGameOver;

    public void SetGameOver()
    {
        if (GameOver) return;   // prevent double trigger

        GameOver = true;
        Debug.Log("GOAL REACHED — GAME OVER");

        OnGameOver?.Invoke();
    }
}