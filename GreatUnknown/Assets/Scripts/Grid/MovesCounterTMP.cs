using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MovesCounterTMP : MonoBehaviour
{
    [SerializeField] private LevelState levelState;
    [SerializeField] private string prefix = "Moves: ";
    [SerializeField] private string zeroMessage = "This path is too long! Restart the game";

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        if (levelState == null)
            levelState = FindObjectOfType<LevelState>();
    }

    private void OnEnable()
    {
        Refresh();

        if (levelState != null)
            levelState.OnGameOver += Refresh;
    }

    private void OnDisable()
    {
        if (levelState != null)
            levelState.OnGameOver -= Refresh;
    }

    private void Update()
    {
        // lightweight polling (safe here since it's just text)
        Refresh();
    }

    private void Refresh()
    {
        if (levelState == null)
            return;

        int moves = levelState.RemainingMoves;

        if (moves <= 0)
        {
            text.text = zeroMessage;
        }
        else
        {
            text.text = prefix + moves;
        }
    }
}