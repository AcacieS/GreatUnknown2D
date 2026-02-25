using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;

    private GameObject currentPlayer;

    // Call this function whenever you want death + respawn
    public void Respawn()
    {
        // Destroy old instance if it exists
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