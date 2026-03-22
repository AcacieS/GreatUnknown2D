using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private bool startOnEnable = false;

    private Coroutine blinkRoutine;
    private bool isBlinking = false;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (startOnEnable)
            StartBlinking();
    }

    private void OnDisable()
    {
        StopBlinking();
    }

    public void StartBlinking()
    {
        if (isBlinking) return;

        isBlinking = true;
        blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    public void StopBlinking()
    {
        if (!isBlinking) return;

        isBlinking = false;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        if (text != null)
            text.enabled = true; // ensure visible at end
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}