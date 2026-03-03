using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FitGridCamera : MonoBehaviour
{
    public int gridWidth = 13;
    public int gridHeight = 13;
    public float cellSize = 1f;
    public Vector2 gridCenter = Vector2.zero; // world center of grid
    public float padding = 0.5f; // optional margin

    private void Start()
    {
        Camera cam = GetComponent<Camera>();

        cam.orthographic = true;

        float worldHeight = gridHeight * cellSize;

        // Orthographic size = half visible height
        cam.orthographicSize = (worldHeight / 2f) + padding;

        // Center camera on grid
        cam.transform.position = new Vector3(
            gridCenter.x,
            gridCenter.y,
            cam.transform.position.z
        );
    }
}