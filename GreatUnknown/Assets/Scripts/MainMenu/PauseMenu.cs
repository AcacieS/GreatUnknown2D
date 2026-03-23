using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private BookViewerUI bookViewerUI;
    [SerializeField] private FaxViewerUI faxViewerUI;

    public static PauseMenu instance {get; private set; }
    public static bool isPaused = false;
    public GameObject pauseMenuUI = null;

    void Awake()
    {
        if (instance == null && instance != this)
        {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // Delete duplicates if we return to the start scene
        }

        if (bookViewerUI == null) { Ext.WarnRefAndDisable("bookViewerUI", this); return; }
        if (faxViewerUI == null) { Ext.WarnRefAndDisable("faxViewerUI", this); return; }
    }

    // Update is called once per frame
    void Update()
    {
        // not allowed in pausing in Main menu
        if (SceneManager.GetActiveScene().name == "Stefa Menu")
        {
            if (pauseMenuUI.activeSelf)
            {
                pauseMenuUI.SetActive(false);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else if (bookViewerUI.gameObject.activeSelf && faxViewerUI.gameObject.activeSelf)
                Pause();
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
}
