using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;

    public void ResumePress()
    {
        pauseUI.SetActive(false);
    }
    public void PausePress()
    {
        pauseUI.SetActive(true);
    }
}
