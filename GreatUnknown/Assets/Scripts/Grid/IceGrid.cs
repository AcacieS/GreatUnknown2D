using UnityEngine;

public enum TileType { Empty, Wall, Stop, Goal, Death }

public class IceGrid
{
    public int Width { get; }
    public int Height { get; }
    private readonly TileType[,] tiles;

    public IceGrid(int width, int height)
    {
        Width = width;
        Height = height;
        tiles = new TileType[width, height];
    }

    public bool InBounds(Vector2Int c) =>
        c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;

    public TileType Get(Vector2Int c) => tiles[c.x, c.y];
    public void Set(Vector2Int c, TileType t) => tiles[c.x, c.y] = t;

private bool IsBlocked(Vector2Int c)
{
    if (!InBounds(c))
    {
        Debug.Log($"BLOCKED: {c} is OUT OF BOUNDS");
        return true;
    }

    if (Get(c) == TileType.Wall)
    {
        Debug.Log($"BLOCKED: {c} is WALL");
        return true;
    }

    return false;
}

    private bool IsStoppingTile(TileType t) => t == TileType.Stop || t == TileType.Goal || t == TileType.Death;

   public Vector2Int Slide(Vector2Int start, Vector2Int dir)
{
    Vector2Int current = start;

    while (true)
    {
        Vector2Int next = current + dir;

        // 🔥 ADD THIS LINE
        Debug.Log($"Slide from {start} dir {dir}: checking {next}, blocked={IsBlocked(next)}, type={Get(next)}");

        if (IsBlocked(next)) return current;

        current = next;

        if (IsStoppingTile(Get(current)))
            return current;
    }
}
}
