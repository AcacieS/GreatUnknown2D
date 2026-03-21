using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject Player;

    // Optional fallback if deadPlayer has no parent
    [SerializeField] private Transform fallbackParent;

    public void OnRespawnButtonClicked()
    {
        var controller = FindFirstObjectByType<IcePlayerController>();
        var currentPlayer = controller != null ? controller.gameObject : Player;

        Respawn(currentPlayer);
    }

    public void Respawn(GameObject deadPlayer)
    {
        Transform parentForRespawn = null;

        if (deadPlayer != null)
            parentForRespawn = deadPlayer.transform.parent;

        if (parentForRespawn == null)
            parentForRespawn = fallbackParent;

        if (deadPlayer != null)
            Destroy(deadPlayer);

        if (playerPrefab == null || startPoint == null)
        {
            Debug.LogError("Restart: Missing playerPrefab or startPoint.");
            return;
        }

        Player = Instantiate(
            playerPrefab,
            startPoint.position,
            Quaternion.identity,
            parentForRespawn
        );
    }

    public void Respawn()
    {
        Respawn(Player);
    }
}