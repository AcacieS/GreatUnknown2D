using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MovesCounterTMP : MonoBehaviour
{
    [SerializeField] private LevelState levelState;
    [SerializeField] private string prefix = "Moves: ";
    [SerializeField] private string zeroMessage = "This path is too long! Restart the game";

    [Header("Zero message transform")]
    [SerializeField] private Vector3 positionForMessage;
    [SerializeField] private Vector3 scaleForMessage;

    private TextMeshProUGUI text;
    private RectTransform rectTransform;

    private Vector3 initialPosition;
    private Vector3 initialScale;

    private bool isShowingZeroMessage = false;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        if (levelState == null)
            levelState = FindObjectOfType<LevelState>();

        // cache initial transform
        initialPosition = rectTransform.anchoredPosition;
        initialScale = rectTransform.localScale;
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
        Refresh();
    }

    private void DisplayZeroMessage()
    {
        text.text = zeroMessage;

        rectTransform.anchoredPosition = positionForMessage;
        rectTransform.localScale = scaleForMessage;

        isShowingZeroMessage = true;
    }

    private void RestoreNormalState(int moves)
    {
        text.text = prefix + moves;

        rectTransform.anchoredPosition = initialPosition;
        rectTransform.localScale = initialScale;

        isShowingZeroMessage = false;
    }

    private void Refresh()
    {
        if (levelState == null)
            return;

        int moves = levelState.RemainingMoves;

        if (moves <= 0)
        {
            if (!isShowingZeroMessage)
                DisplayZeroMessage();
        }
        else
        {
            if (isShowingZeroMessage)
                RestoreNormalState(moves);
            else
                text.text = prefix + moves;
        }
    }
}