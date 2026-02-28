using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject playerPrefab;

    // If you already had a field like this, keep it:
    [SerializeField] private GameObject Player;

    // ✅ UI Button can call this (no parameters)
    public void OnRespawnButtonClicked()
    {
        // Prefer respawning the *current* player in the scene, in case Player reference is stale
        var controller = FindFirstObjectByType<IcePlayerController>();
        var currentPlayer = controller != null ? controller.gameObject : Player;

        Respawn(currentPlayer);
    }

    // ✅ respawn the specific instance that died
    public void Respawn(GameObject deadPlayer)
    {
        if (deadPlayer != null)
            Destroy(deadPlayer);

        if (playerPrefab == null || startPoint == null)
        {
            Debug.LogError("Restart: Missing playerPrefab or startPoint.");
            return;
        }

        Player = Instantiate(playerPrefab, startPoint.position, Quaternion.identity);
    }

    // ✅ old API still works
    public void Respawn()
    {
        Respawn(Player);
    }
}