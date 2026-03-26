using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private VideoPlayer cutscene;

    void Awake()
    {
        cutscene = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        canvas.gameObject.SetActive(true);
        cutscene.started += OnCutsceneStart;
        cutscene.loopPointReached += OnCutsceneFinish;
    }

    void OnDisable()
    {
        cutscene.started -= OnCutsceneStart;
        cutscene.loopPointReached -= OnCutsceneFinish;
    }

    private void OnCutsceneStart(VideoPlayer p) => canvas.gameObject.SetActive(false);
    private void OnCutsceneFinish(VideoPlayer p) => Application.Quit();
}
