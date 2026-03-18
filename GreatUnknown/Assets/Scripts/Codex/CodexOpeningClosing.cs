using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CodexOpeningClosing : MonoBehaviour
{
    [SerializeField] private GameObject Codex;
    [SerializeField] private Image overlay;

    [Header("Fade Settings")]
    [SerializeField] private float targetAlpha = 0.6f;
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine fadeRoutine;

    void OnEnable()
    {
        // Reset alpha to 0 instantly
        SetAlpha(0f);

        // Start fade-in
        StartFade(targetAlpha);
    }

    public void OnCodexOverlayClick()
    {
        if (!Codex.activeSelf) return;

        // Fade out, then disable codex
        StartFade(0f, () => Codex.SetActive(false));
    }

    private void StartFade(float target, System.Action onComplete = null)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeAlpha(target, onComplete));
    }

    private IEnumerator FadeAlpha(float target, System.Action onComplete)
    {
        float startAlpha = overlay.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            float newAlpha = Mathf.Lerp(startAlpha, target, t);
            SetAlpha(newAlpha);

            yield return null;
        }

        SetAlpha(target);
        onComplete?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        Color c = overlay.color;
        c.a = alpha;
        overlay.color = c;
    }
}