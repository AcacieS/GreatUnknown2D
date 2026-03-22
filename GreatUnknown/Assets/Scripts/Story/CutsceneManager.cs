using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private string endScene = "Jonathan End";
    private VideoPlayer cutscene;

    void Awake()
    {
        cutscene = GetComponent<VideoPlayer>();

        cutscene.loopPointReached += OnCutsceneFinish;
    }

    private void OnCutsceneFinish(VideoPlayer source)
    {
        SceneManager.LoadSceneAsync(endScene);
    }
}
