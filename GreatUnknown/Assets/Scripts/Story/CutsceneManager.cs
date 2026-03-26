using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private float endPause = 1.5f;
    [SerializeField] private Canvas canvas;
    private VideoPlayer cutscene;

    void Awake()
    {
        cutscene = GetComponent<VideoPlayer>();
        cutscene.loopPointReached += (unusedSource) => StartCoroutine(OnCutsceneFinish());
    }

    void OnEnable()
    {
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator OnCutsceneFinish()
    {
        canvas.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(endPause);
        Application.Quit();
    }
}
