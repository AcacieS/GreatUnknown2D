using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Single source of truth at runtime:
/// - Holds the active IceGrid + PlayerStart
/// - Can be built from ASCII (debug) OR injected from a Tilemap builder
/// - Optionally spawns visuals and colliders
/// </summary>
public class IceGridBehaviour : MonoBehaviour
{
    [Header("Tilemap Mapping (preferred when using Tilemap injection)")]
    public Tilemap mappingTilemap;      // assign at runtime via injector
    public Vector3Int mappingBoundsMin; // assign at runtime via injector
    [Header("Grid Runtime")]
    [Tooltip("If true, builds from the ASCII level in Awake unless a grid was injected first.")]
    [SerializeField] private bool buildFromAsciiIfNotInjected = true;

    [Tooltip("If true, spawns prefabs for every cell after building/injection.")]
    [SerializeField] private bool spawnVisuals = true;

    [Tooltip("If true, ensures walls have colliders (adds BoxCollider2D if missing).")]
    [SerializeField] private bool autoAddWallColliders = true;

    [Tooltip("Centers the grid around (0,0) by setting origin accordingly.")]
    [SerializeField] private bool centerMap = true;

    [Header("Grid Settings (used for visuals/world mapping)")]
    public float cellSize = 1f;
    public Vector2 origin = Vector2.zero;

    [Header("Tile Prefabs (optional visuals)")]
    public GameObject wallPrefab;
    public GameObject icePrefab;   // TileType.Empty
    public GameObject stopPrefab;
    public GameObject goalPrefab;  // optional
    public GameObject deathPrefab; // TileType.Death (hazard)

    [Header("ASCII Level (debug fallback)")]
    [TextArea(6, 20)]
    public string level =
@"##########
#....S...#
#..##....#
#..P...G.#
#........#
##########";

    // --- Public API used by player/controller ---
    public IceGrid Grid { get; private set; }
    public Vector2Int PlayerStart { get; private set; }

    // Internal
    private bool _gridInjected = false;
    private Transform _tilesParent;

    private void Awake()
    {
        // If something injected the grid before Awake ends, we don't build ASCII.
        if (!_gridInjected && buildFromAsciiIfNotInjected)
        {
            BuildFromAscii(level);
            PostBuildOrInject();
        }
        else if (_gridInjected)
        {
            // injection already called PostBuildOrInject()
        }
        else
        {
            // Nothing built and ASCII disabled: you will have no grid.
            Debug.LogWarning($"{nameof(IceGridBehaviour)} has no grid. Enable ASCII fallback or inject a grid at runtime.");
        }
    }

    /// <summary>
    /// Inject a grid produced elsewhere (e.g., IceGridFromTilemap).
    /// This makes IceGridBehaviour the single runtime owner.
    /// </summary>
    public void SetGrid(IceGrid grid, Vector2Int playerStart)
    {
        if (grid == null)
        {
            Debug.LogError("SetGrid called with null grid.");
            return;
        }

        Grid = grid;
        PlayerStart = playerStart;
        _gridInjected = true;

        PostBuildOrInject();
    }

    // --- World/Grid mapping helpers ---
public Vector3 GridToWorldCenter(Vector2Int c)
{
    // If we have a tilemap mapping, use it (exact alignment)
    if (mappingTilemap != null)
    {
        Vector3Int cell = new Vector3Int(mappingBoundsMin.x + c.x, mappingBoundsMin.y + c.y, 0);
        return mappingTilemap.GetCellCenterWorld(cell);
    }

    // Fallback to old math mapping (ASCII mode)
    return origin + new Vector2((c.x + 0.5f) * cellSize, (c.y + 0.5f) * cellSize);
}

    public Vector2Int WorldToGrid(Vector2 world)
    {
        Vector2 local = world - origin;
        int x = Mathf.FloorToInt(local.x / cellSize);
        int y = Mathf.FloorToInt(local.y / cellSize);
        return new Vector2Int(x, y);
    }

    // --- Build paths ---
    private void BuildFromAscii(string ascii)
    {
        var lines = ascii.Replace("\r", "").Split('\n');
        int h = lines.Length;
        int w = lines[0].Length;

        Grid = new IceGrid(w, h);

        // Parse top->bottom; map top line to highest y
        for (int row = 0; row < h; row++)
        {
            string line = lines[row];
            int y = h - 1 - row;

            for (int x = 0; x < w; x++)
            {
                char ch = line[x];
                Vector2Int c = new Vector2Int(x, y);

                switch (ch)
                {
                    case '#': Grid.Set(c, TileType.Wall); break;
                    case '.': Grid.Set(c, TileType.Empty); break;
                    case 'S': Grid.Set(c, TileType.Stop); break;
                    case 'G': Grid.Set(c, TileType.Goal); break;
                    case 'X': Grid.Set(c, TileType.Death); break;
                    case 'P':
                        Grid.Set(c, TileType.Empty);
                        PlayerStart = c;
                        break;
                    default:
                        Grid.Set(c, TileType.Empty);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Runs after either ASCII build or injection.
    /// Handles origin, visuals, and collider correctness.
    /// </summary>
    private void PostBuildOrInject()
    {
        if (Grid == null)
        {
            Debug.LogError("PostBuildOrInject called but Grid is null.");
            return;
        }

        if (centerMap)
        {
            origin = new Vector2(
                -Grid.Width * cellSize / 2f,
                -Grid.Height * cellSize / 2f
            );
        }

        if (spawnVisuals)
            SpawnVisuals();
    }

    private void SpawnVisuals()
    {
        if (_tilesParent != null)
            Destroy(_tilesParent.gameObject);

        _tilesParent = new GameObject("Tiles").transform;
        _tilesParent.SetParent(transform, false);

        for (int x = 0; x < Grid.Width; x++)
        for (int y = 0; y < Grid.Height; y++)
        {
            var c = new Vector2Int(x, y);
            var t = Grid.Get(c);

            GameObject prefab = t switch
            {
                TileType.Wall => wallPrefab,
                TileType.Stop => stopPrefab,
                TileType.Goal => goalPrefab != null ? goalPrefab : stopPrefab,
                TileType.Death => deathPrefab != null ? deathPrefab : icePrefab,
                _ => icePrefab
            };

            if (prefab == null) continue;

            var go = Instantiate(prefab, GridToWorldCenter(c), Quaternion.identity, _tilesParent);
            go.name = $"{t} ({x},{y})";

            // Ensure walls actually block the player (common “passes through boundaries” issue)
            if (autoAddWallColliders && t == TileType.Wall)
            {
                // If prefab already has a collider, this does nothing.
                if (go.GetComponent<Collider2D>() == null)
                {
                    var bc = go.AddComponent<BoxCollider2D>();
                    bc.isTrigger = false;
                    // Size roughly one cell. Adjust if your sprites are different scale.
                    bc.size = Vector2.one * cellSize;
                }
            }
        }
    }
}