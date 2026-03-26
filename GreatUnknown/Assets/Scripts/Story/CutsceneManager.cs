using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer), typeof(RawImage))]
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private float endPause = 1.5f;
    private CustomRenderTexture texture;
    private VideoPlayer player;
    private RawImage output;

    void Awake()
    {
        player = GetComponent<VideoPlayer>();
        output = GetComponent<RawImage>();

        texture = new CustomRenderTexture(Screen.width, Screen.height);
        texture.initializationColor = new Color(0, 0, 0, 1);

        player.targetTexture = texture;
        output.texture = texture;
    }

    void OnEnable()
    {
        player.loopPointReached += OnCutsceneFinish;
    }

    void OnDisable()
    {
        player.loopPointReached -= OnCutsceneFinish;
    }
    
    private void OnCutsceneFinish(VideoPlayer p) => StartCoroutine(EndGameRoutine());

    private IEnumerator EndGameRoutine()
    {
        yield return new WaitForSecondsRealtime(endPause);
        Application.Quit();
    }
}
