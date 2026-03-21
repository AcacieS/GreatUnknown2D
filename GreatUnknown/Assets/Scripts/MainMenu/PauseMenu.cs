using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
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
            else 
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
