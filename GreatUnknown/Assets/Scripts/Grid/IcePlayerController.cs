using UnityEngine;

public class IcePlayerController : MonoBehaviour
{
    [SerializeField] private IceGridFromTilemap gridSource;
    [SerializeField] private Restart restart;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private LevelState levelState;

    private IceGrid grid;
    private Vector2Int pos;
    private Vector3 targetWorld;
    private bool isMoving;
    private bool isRespawning; // guard

    private void Awake()
    {
        if (gridSource == null) gridSource = FindFirstObjectByType<IceGridFromTilemap>();
        if (restart == null)   restart   = FindFirstObjectByType<Restart>();
        if (levelState == null) levelState = FindFirstObjectByType<LevelState>();
    }

    private void Start()
    {
        if (gridSource == null)
        {
            Debug.LogError("IcePlayerController: No IceGridFromTilemap found in scene.");
            enabled = false;
            return;
        }

        if (gridSource.Grid == null) gridSource.BuildGridFromTilemaps();
        grid = gridSource.Grid;

        pos = gridSource.PlayerStart;
        targetWorld = gridSource.GridIndexToWorldCenter(pos);
        transform.position = targetWorld;
    }

    private void Update()
    {
        if (grid == null || gridSource == null) return;
        if (isRespawning) return;
        if (levelState != null && levelState.GameOver) return;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorld, moveSpeed * Time.deltaTime);

            if ((transform.position - targetWorld).sqrMagnitude < 0.0001f)
            {
                transform.position = targetWorld;
                isMoving = false;

                TileType landed = grid.Get(pos);

                if (landed == TileType.Death && restart != null)
                {
                    isRespawning = true;
                    restart.Respawn(gameObject);
                    return;
                }

                if (landed == TileType.Goal && levelState != null)
                {
                    levelState.SetGameOver();
                    return;
                }
            }

            return; // no input mid-move
        }

        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;
        else return;

        Vector2Int newPos = grid.Slide(pos, dir);
        if (newPos == pos) return;

        pos = newPos;
        targetWorld = gridSource.GridIndexToWorldCenter(pos);
        isMoving = true;
    }
}