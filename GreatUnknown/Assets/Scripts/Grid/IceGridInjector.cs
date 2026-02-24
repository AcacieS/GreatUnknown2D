using UnityEngine;
using UnityEngine.Tilemaps;

public class IceGridInjector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IceGridBehaviour gridRoot;        // your Gridroot object
    [SerializeField] private IceGridFromTilemap tilemapBuilder; // the builder (walls/stop/goal tilemaps)

    private void Awake()
    {
        if (gridRoot == null)
        {
            Debug.LogError("IceGridInjector: gridRoot is not assigned.");
            return;
        }
        if (tilemapBuilder == null)
        {
            Debug.LogError("IceGridInjector: tilemapBuilder is not assigned.");
            return;
        }

        // 1) Build the grid from tilemaps (builder already does this in its Awake,
        //    but calling it here makes the order explicit and safe)
        tilemapBuilder.BuildGridFromTilemaps();

        if (tilemapBuilder.Grid == null)
        {
            Debug.LogError("IceGridInjector: Builder Grid is null after BuildGridFromTilemaps(). Check tilemap assignments/bounds.");
            return;
        }

        // 2) Align Gridroot mapping to the Tilemap (so GridToWorldCenter() is correct)
        AlignGridRootToTilemap(tilemapBuilder.wallsTilemap, tilemapBuilder.BoundsMin);

        // 3) Inject into the runtime owner
        gridRoot.SetGrid(tilemapBuilder.Grid, tilemapBuilder.PlayerStart);
        
    }

    private void AlignGridRootToTilemap(Tilemap walls, Vector3Int boundsMin)
    {
        if (walls == null) return;

        Vector3 c00 = walls.GetCellCenterWorld(boundsMin);                  // center of builder index (0,0)
        Vector3 c10 = walls.GetCellCenterWorld(boundsMin + Vector3Int.right); // center of (1,0)

        float cellSize = Mathf.Abs(c10.x - c00.x);
        if (cellSize <= 0.0001f) cellSize = 1f;

        // We want: GridToWorldCenter(0,0) == c00
        // GridToWorldCenter(0,0) = origin + (0.5*cellSize, 0.5*cellSize)
        Vector2 origin = new Vector2(c00.x, c00.y) - new Vector2(0.5f * cellSize, 0.5f * cellSize);

        gridRoot.cellSize = cellSize;
        gridRoot.origin = origin;
    }
}