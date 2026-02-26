using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private GameObject playerPrefab;

    // If you already had a field like this, keep it:
    [SerializeField]
    private GameObject Player;

    // ✅ NEW: respawn the specific instance that died
    public void Respawn(GameObject deadPlayer)
    {
        if (deadPlayer != null)
        {
            deadPlayer.SetActive(false); // hard stop immediately
            Destroy(deadPlayer);
        }

        if (playerPrefab == null || startPoint == null)
        {
            Debug.LogError("Restart: Missing playerPrefab or startPoint.");
            return;
        }

        Player = Instantiate(playerPrefab, startPoint.position, Quaternion.identity);
    }

    // ✅ KEEP: old API still works
    public void Respawn()
    {
        Respawn(Player);
    }
}
