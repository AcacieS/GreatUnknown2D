using System;
using UnityEngine;

public class Grid2D<T>
{
    public readonly int Width;
    public readonly int Height;
    public readonly float CellSize;
    public readonly Vector2 Origin; // bottom-left corner in world space (2D)

    private readonly T[,] _cells;

    public Grid2D(int width, int height, float cellSize, Vector2 origin, Func<int,int,T> createCell = null)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        Origin = origin;

        _cells = new T[width, height];

        if (createCell != null)
        {
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                _cells[x, y] = createCell(x, y);
        }
    }

    public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

    public bool TryGet(int x, int y, out T value)
    {
        if (!InBounds(x, y))
        {
            value = default;
            return false;
        }
        value = _cells[x, y];
        return true;
    }

    public bool TrySet(int x, int y, T value)
    {
        if (!InBounds(x, y)) return false;
        _cells[x, y] = value;
        return true;
    }

    // World position of the *center* of a cell
    public Vector2 GridToWorldCenter(int x, int y)
    {
        return Origin + new Vector2((x + 0.5f) * CellSize, (y + 0.5f) * CellSize);
    }

    // World -> grid coordinate (floor)
    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector2 local = worldPos - Origin;
        int x = Mathf.FloorToInt(local.x / CellSize);
        int y = Mathf.FloorToInt(local.y / CellSize);
        return new Vector2Int(x, y);
    }

    public Vector2Int[] Get4Neighbors(Vector2Int c)
    {
        return new[]
        {
            new Vector2Int(c.x + 1, c.y),
            new Vector2Int(c.x - 1, c.y),
            new Vector2Int(c.x, c.y + 1),
            new Vector2Int(c.x, c.y - 1),
        };
    }
}
