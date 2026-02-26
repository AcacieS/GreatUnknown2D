using UnityEngine;

public class LevelState : MonoBehaviour
{
    public bool GameOver { get; private set; }

    public void SetGameOver()
    {
        GameOver = true;
        Debug.Log("GOAL REACHED — GAME OVER");
    }
}