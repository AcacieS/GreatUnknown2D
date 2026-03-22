using UnityEngine;

public class Restart : MonoBehaviour
{
    [Header("Local Level References")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject Player;
    [SerializeField] private IceGridFromTilemap gridSource;
    [SerializeField] private LevelState levelState;

    [Header("Optional Shared UI Routing")]
    [SerializeField] private IceSlidingGameSwitcher iceSlidingGameSwitcher;

    public void OnRespawnButtonClicked()
    {
        Restart targetRestart = ResolveActiveRestart();

        if (targetRestart == null)
        {
            Debug.LogError("Restart: Could not resolve active Restart for shared button.");
            return;
        }

        targetRestart.RespawnCurrentPlayer();
    }

    public void RespawnCurrentPlayer()
    {
        Respawn(Player);
    }

    public void Respawn(GameObject deadPlayer)
    {
        if (playerPrefab == null || startPoint == null)
        {
            Debug.LogError("Restart: Missing playerPrefab or startPoint.");
            return;
        }

        Transform parentForRespawn = null;

        if (deadPlayer != null)
            parentForRespawn = deadPlayer.transform.parent;

        if (parentForRespawn == null)
            parentForRespawn = startPoint.parent;

        if (deadPlayer != null)
            Destroy(deadPlayer);

        Player = Instantiate(
            playerPrefab,
            startPoint.position,
            Quaternion.identity,
            parentForRespawn
        );

        IcePlayerController controller = Player.GetComponent<IcePlayerController>();
        if (controller == null)
        {
            Debug.LogError("Restart: Spawned player prefab has no IcePlayerController.");
            return;
        }

        if (gridSource == null)
            gridSource = GetComponentInParent<IceGridFromTilemap>(true);

        if (levelState == null)
            levelState = GetComponentInParent<LevelState>(true);

        controller.InjectReferences(gridSource, this, levelState);
    }

    public void Respawn()
    {
        Respawn(Player);
    }

    private Restart ResolveActiveRestart()
    {
        // Case 1: this Restart is already the local one inside the active level
        if (IsInsideActiveLevel())
            return this;

        // Case 2: shared outer UI button routes through switcher
        if (iceSlidingGameSwitcher == null)
            iceSlidingGameSwitcher = FindFirstObjectByType<IceSlidingGameSwitcher>();

        if (iceSlidingGameSwitcher == null)
            return this;

        GameObject currentGame = iceSlidingGameSwitcher.GetCurrentGame();
        if (currentGame == null)
            return null;

        Restart activeRestart = currentGame.GetComponentInChildren<Restart>(true);
        return activeRestart;
    }

    private bool IsInsideActiveLevel()
    {
        if (iceSlidingGameSwitcher == null)
            iceSlidingGameSwitcher = FindFirstObjectByType<IceSlidingGameSwitcher>();

        if (iceSlidingGameSwitcher == null)
            return true;

        GameObject currentGame = iceSlidingGameSwitcher.GetCurrentGame();
        if (currentGame == null)
            return true;

        return transform.IsChildOf(currentGame.transform) || gameObject == currentGame;
    }
}