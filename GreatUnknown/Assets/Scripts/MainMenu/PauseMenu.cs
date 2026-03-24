using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, TWTWControls.IPauseActions
{
    [SerializeField] private InputActionReference escapeActionReference;
    [SerializeField] private BookViewerUI bookViewerUI;
    [SerializeField] private FaxViewerUI faxViewerUI;
    [SerializeField] private GameObject pauseMenuUI = null;

    private TWTWControls controlMaps;
    public static PauseMenu instance {get; private set; }
    private bool canPause = true;
    public static bool isPaused = false;
    public string mainMenuScene; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance); // Delete duplicates if we return to the start scene
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        controlMaps = new TWTWControls();
        controlMaps.Pause.AddCallbacks(this);

        if (bookViewerUI == null) { Ext.WarnRef("bookViewerUI", this); return; }
        if (faxViewerUI == null) { Ext.WarnRef("faxViewerUI", this); return; }
    }

    // Update is called once per frame
    void Update()
    {
        // not allowed in pausing in Main menu
        if (SceneManager.GetActiveScene().name == mainMenuScene)
        {
            if (pauseMenuUI.activeSelf)
            {
                pauseMenuUI.SetActive(false);
                return;
            }
        }

        if (bookViewerUI != null && faxViewerUI != null)
        {
            canPause = !bookViewerUI.gameObject.activeSelf && !faxViewerUI.transform.parent.gameObject.activeSelf;
        } else
        {
            canPause = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stefa Menu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    #region Action Bindings for IPauseActions

    public void OnTogglePause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused)
                Resume();
            else if (canPause)
                Pause();
        }
    }

    void OnDestroy()
    {
        controlMaps.Dispose();
    }

    void OnEnable()
    {
        controlMaps.Pause.Enable();
    }

    void OnDisable()
    {
        controlMaps.Pause.Disable();
    }

    #endregion Action Bindings for IPauseActions
}
