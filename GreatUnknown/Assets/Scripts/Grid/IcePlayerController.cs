using UnityEngine;
using UnityEngine.InputSystem;

public class IcePlayerController : MonoBehaviour, TWTWControls.IIceSlidingActions
{
    [SerializeField]
    private IceGridFromTilemap gridSource;

    [SerializeField]
    private Restart restart;

    [SerializeField]
    private float moveSpeed = 6f;

    [SerializeField]
    private LevelState levelState;

    [SerializeField]
    private Transform submarineVisual;

    private IceGrid grid;
    private Vector2Int pos;
    private Vector3 targetWorld;
    private bool isMoving;
    private bool isRespawning;
    //For sound(plays once);
    private bool canPlaySound = true;

    private TWTWControls controlMaps;
    private Vector2Int dir = Vector2Int.zero;

    public void InjectReferences(
        IceGridFromTilemap injectedGridSource,
        Restart injectedRestart,
        LevelState injectedLevelState
    )
    {
        gridSource = injectedGridSource;
        restart = injectedRestart;
        levelState = injectedLevelState;
    }

    private void Awake()
    {
        if (submarineVisual == null)
            submarineVisual = transform;

        controlMaps = new TWTWControls();
        controlMaps.IceSliding.AddCallbacks(this);
    }

    private void Start()
    {
        if (gridSource == null)
        {
        
            Debug.LogError("IcePlayerController: gridSource was not injected.");
            enabled = false;
            return;
        }

        if (restart == null)
        {
            Debug.LogError("IcePlayerController: restart was not injected.");
            enabled = false;
            return;
        }

        if (gridSource.Grid == null)
            gridSource.BuildGridFromTilemaps();

        grid = gridSource.Grid;

        if (grid == null)
        {
            Debug.LogError("IcePlayerController: gridSource built no grid.");
            enabled = false;
            return;
        }

        pos = gridSource.PlayerStart;
        targetWorld = gridSource.GridIndexToWorldCenter(pos);
        transform.position = targetWorld;
        FaceDirection(Vector2Int.up);
    }

    private void Update()
    {
        if (grid == null || gridSource == null)
            return;
        if (isRespawning)
            return;
        if (levelState != null && levelState.GameOver)
            return;

        if (isMoving)
        {
            //Play the vroom thing once
            if(canPlaySound) {
                SoundManager.instance?.PlaySound("minigame_go");
                canPlaySound = !canPlaySound;
            }
        
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWorld,
                moveSpeed * Time.deltaTime
            );

            if ((transform.position - targetWorld).sqrMagnitude < 0.0001f)
            {
                transform.position = targetWorld;
                canPlaySound = true; 
                isMoving = false;


                TileType landed = grid.Get(pos);

                if (landed == TileType.Death && restart != null)
                {
                    isRespawning = true;
                    SoundManager.instance?.PlaySound("minigame_lose");
                    restart.Respawn(gameObject);
                    return;
                }

                if (landed == TileType.Goal && levelState != null)
                {
                    SoundManager.instance?.PlaySound("minigame_win");
                    levelState.SetGameOver();
                    return;
                }
            }

            return;
        }

        switch (dir)
        {
            case Vector2Int.up:
            case Vector2Int.down:
            case Vector2Int.left:
            case Vector2Int.right:
                Vector2Int newPos = grid.Slide(pos, dir);
                if (newPos == pos)
                    return;

                FaceDirection(dir);
                pos = newPos;
                targetWorld = gridSource.GridIndexToWorldCenter(pos);
                isMoving = true;
                break;
            default:
                return;
        }
    }

    private void FaceDirection(Vector2Int dir)
    {
        
        if (submarineVisual == null)
            return;

        if (dir == Vector2Int.up)
            submarineVisual.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (dir == Vector2Int.right)
            submarineVisual.rotation = Quaternion.Euler(0f, 0f, -90f);
        else if (dir == Vector2Int.down)
            submarineVisual.rotation = Quaternion.Euler(0f, 0f, 180f);
        else if (dir == Vector2Int.left)
            submarineVisual.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    #region Action Bindings for IIceSlidingActions

    public void OnMove(InputAction.CallbackContext context)
    {
        dir = Vector2Int.RoundToInt(context.ReadValue<Vector2>());
    }

    void OnDestroy()
    {
        controlMaps.Dispose();
    }

    void OnEnable()
    {
        controlMaps.IceSliding.Enable();
    }

    void OnDisable()
    {
        controlMaps.IceSliding.Disable();
    }

    #endregion Action Bindings for IIceSlidingActions
}
