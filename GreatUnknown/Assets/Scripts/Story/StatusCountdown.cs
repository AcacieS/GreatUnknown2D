using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class StatusCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int count = 49;
    [SerializeField] private UnityEvent countdownTerminated;

    // Template is extracted from `text.text`
    private string template;

    void Awake()
    {
        template = text.text.Substring(0, text.text.Length - 2);
    }

    void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        for (;count > 0; --count)
        {
            if (count > 9)
            {
                text.text = template + count;
            } else
            {
                text.text = template + "0" + count;
            }
            yield return new WaitForSeconds(1);
        }
        countdownTerminated?.Invoke();
    }
}
