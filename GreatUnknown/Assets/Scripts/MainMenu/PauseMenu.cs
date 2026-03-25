using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private BookViewerUI bookViewerUI;
    [SerializeField] private FaxViewerUI faxViewerUI;
    [SerializeField] private GameObject pauseMenuUI = null;

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

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else if (canPause)
                Pause();
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
        SceneManager.LoadScene(mainMenuScene);
    }
}
