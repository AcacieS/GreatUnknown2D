using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Builds an IceGrid (see Grid/IceGrid.cs) from one or more Tilemaps.
/// - wallsTilemap: any tile = Wall
/// - stopTilemap: any tile = Stop (optional)
/// - goalTilemap: any tile = Goal (optional)
///
/// Coordinates:
/// - We index the grid as (0..Width-1, 0..Height-1)
/// - BoundsMin stores the bottom-left cell coordinate of the combined bounds.
/// </summary>
public class IceGridFromTilemap : MonoBehaviour
{
    [Header("Tilemap Sources")]
    [Tooltip("Any tile placed here becomes a Wall (blocks sliding). Required.")]
    public Tilemap wallsTilemap;

    [Tooltip("Any tile placed here becomes a Stop tile (stops sliding). Optional.")]
    public Tilemap stopTilemap;

    [Tooltip("Any tile placed here becomes a Goal tile (stops sliding + win). Optional.")]
    public Tilemap goalTilemap;

    [Header("Start")]
    [Tooltip("If assigned, we compute the player's start cell from this marker's world position.")]
    public Transform startMarker;

    public IceGrid Grid { get; private set; }
    public Vector2Int PlayerStart { get; private set; }

    /// <summary>Bottom-left cell coordinate (inclusive) used as origin for 0-based indexing.</summary>
    public Vector3Int BoundsMin { get; private set; }

    /// <summary>Combined bounds (inclusive min, exclusive max) used for indexing.</summary>
    public BoundsInt CombinedBounds { get; private set; }

    private void Awake()
    {
        BuildGridFromTilemaps();
        
    }

 public void BuildGridFromTilemaps()
{
    if (wallsTilemap == null)
    {
        Debug.LogError("IceGridFromTilemap: Assign at least a Walls Tilemap.");
        return;
    }

    // 🔥 ADD THIS — VERY IMPORTANT
    wallsTilemap.CompressBounds();
    if (stopTilemap != null) stopTilemap.CompressBounds();
    if (goalTilemap != null) goalTilemap.CompressBounds();

    // Now compute bounds AFTER compression
    CombinedBounds = wallsTilemap.cellBounds;

    if (stopTilemap != null)
        CombinedBounds = Union(CombinedBounds, stopTilemap.cellBounds);

    if (goalTilemap != null)
        CombinedBounds = Union(CombinedBounds, goalTilemap.cellBounds);

    BoundsMin = CombinedBounds.min;

    int width = CombinedBounds.size.x;
    int height = CombinedBounds.size.y;
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("IceGridFromTilemap: Combined tilemap bounds are empty (no cells).");
            return;
        }

        Grid = new IceGrid(width, height);

        // Fill the grid by sampling each cell in the combined bounds.
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3Int cell = new Vector3Int(BoundsMin.x + x, BoundsMin.y + y, 0);

            // Priority: Wall blocks everything, so it wins.
            if (wallsTilemap.HasTile(cell))
            {
                Grid.Set(new Vector2Int(x, y), TileType.Wall);
                continue;
            }

            // Goal/Stop are non-blocking but stop sliding in IceGrid.Slide.
            if (goalTilemap != null && goalTilemap.HasTile(cell))
            {
                Grid.Set(new Vector2Int(x, y), TileType.Goal);
                continue;
            }

            if (stopTilemap != null && stopTilemap.HasTile(cell))
            {
                Grid.Set(new Vector2Int(x, y), TileType.Stop);
                continue;
            }

            Grid.Set(new Vector2Int(x, y), TileType.Empty);
            Debug.Log("Built grid from tilemap with size: " + width + "x" + height);
        }

        // Determine player start from marker.
        if (startMarker != null)
        {
            Vector3Int startCell = wallsTilemap.WorldToCell(startMarker.position);
            Vector2Int idx = CellToGridIndex(startCell);

            // Keep it safe: clamp into bounds so you don't crash if marker is outside.
            idx = new Vector2Int(
                Mathf.Clamp(idx.x, 0, width - 1),
                Mathf.Clamp(idx.y, 0, height - 1)
            );

            PlayerStart = idx;
        }
        else
        {
            PlayerStart = new Vector2Int(0, 0);
        }
    }

    // Convert Tilemap cell coords -> our 0..Width-1 indexing
    public Vector2Int CellToGridIndex(Vector3Int cell)
    {
        return new Vector2Int(cell.x - BoundsMin.x, cell.y - BoundsMin.y);
    }

    // Convert our indexing -> world center position of a cell (uses wallsTilemap as reference)
    public Vector3 GridIndexToWorldCenter(Vector2Int idx)
    {
        Vector3Int cell = new Vector3Int(BoundsMin.x + idx.x, BoundsMin.y + idx.y, 0);
        return wallsTilemap.GetCellCenterWorld(cell);
    }

    private static BoundsInt Union(BoundsInt a, BoundsInt b)
    {
        Vector3Int min = new Vector3Int(
            Mathf.Min(a.min.x, b.min.x),
            Mathf.Min(a.min.y, b.min.y),
            Mathf.Min(a.min.z, b.min.z)
        );
        Vector3Int max = new Vector3Int(
            Mathf.Max(a.max.x, b.max.x),
            Mathf.Max(a.max.y, b.max.y),
            Mathf.Max(a.max.z, b.max.z)
        );

        // BoundsInt takes position=min and size=(max-min)
        return new BoundsInt(min, max - min);
    }
}
