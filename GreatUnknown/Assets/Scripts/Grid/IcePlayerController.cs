using UnityEngine;

public class IcePlayerController : MonoBehaviour
{
    public IceGridBehaviour gridBehaviour;
    public Restart restart;

    [Header("Movement")]
    public float moveSpeed = 10f; // visual smoothing

    private Vector2Int gridPos;
    private Vector3 targetWorldPos;
    private bool isMoving;
    private bool wasMoving;

    private void Start()
    {
        if (gridBehaviour == null)
            gridBehaviour = FindObjectOfType<IceGridBehaviour>();

        if (restart == null)
            restart = FindObjectOfType<Restart>();

        if (restart != null)
            restart.RegisterCurrentPlayer(gameObject);

        gridPos = gridBehaviour.PlayerStart;
        targetWorldPos = gridBehaviour.GridToWorldCenter(gridPos);
        transform.position = targetWorldPos;
    }

    private void Update()
    {
        // Smooth move
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
        isMoving = (transform.position - targetWorldPos).sqrMagnitude > 0.0001f;

        // If we just arrived on a cell this frame, resolve tile effects.
        if (wasMoving && !isMoving)
            OnArrivedAtCell();
        wasMoving = isMoving;

        if (isMoving) return; // lock input until we arrive

        Vector2Int dir = ReadDir();
        if (dir == Vector2Int.zero) return;

        Vector2Int end = gridBehaviour.Grid.Slide(gridPos, dir);
        if (end != gridPos)
        {
            gridPos = end;
            targetWorldPos = gridBehaviour.GridToWorldCenter(gridPos);
        }
    }


    private void OnArrivedAtCell()
    {
        if (gridBehaviour == null || gridBehaviour.Grid == null) return;
        TileType t = gridBehaviour.Grid.Get(gridPos);
        if (t == TileType.Death && restart != null)
        {
            restart.Respawn();
        }
    }

    private Vector2Int ReadDir()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return Vector2Int.right;
        return Vector2Int.zero;
    }
}
