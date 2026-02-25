using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;

    [SerializeField] private GameObject currentPlayer; // optional: assign if player already exists in scene


    /// <summary>
    /// Optional: lets the player register itself so Respawn() knows what to destroy.
    /// </summary>
    public void RegisterCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
    }

    // Call this function whenever you want death + respawn
    public void Respawn()
    {
        // Destroy old instance if it exists
        if (currentPlayer == null)
        {
            // Try to find an existing player in the scene (optional)
            var tagged = GameObject.FindGameObjectWithTag("Player");
            if (tagged != null) currentPlayer = tagged;
        }

        if (currentPlayer != null)
            Destroy(currentPlayer);

        // Instantiate new instance at start position
        currentPlayer = Instantiate(
            playerPrefab,
            startPoint.position,
            startPoint.rotation
        );
    }
}