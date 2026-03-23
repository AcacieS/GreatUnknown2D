using UnityEngine;

public class EnableOnGameOver : MonoBehaviour
{
    [SerializeField]
    private LevelState levelState;

    [SerializeField]
    private GameObject target;

    private void Awake()
    {
        if (levelState == null)
            levelState = FindFirstObjectByType<LevelState>();
    }

    private void OnEnable()
    {
        if (levelState != null)
            levelState.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        if (levelState != null)
            levelState.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        Debug.Log("HandleGameOVerGotHandled");
        if (target != null)
            target.SetActive(true);
    }
}
