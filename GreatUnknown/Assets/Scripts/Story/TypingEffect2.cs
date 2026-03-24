using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingEffect2 : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected string textToShow;
    [SerializeField] private GameObject[] toClose;

    [SerializeField] protected float typingSpeed = 0.05f;
    [SerializeField] protected float waitSecondAfterTyping = 3f;

    public virtual void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    public virtual void OnEnable()
    {
        StopAllCoroutines();
        WriteText();
    }

    public IEnumerator TypeText()
    {
        if (text == null || string.IsNullOrEmpty(textToShow))
            yield break;

        text.text = "";

        foreach (char c in textToShow)
        {

            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        FinishText();
    }

    public virtual void FinishText()
    {
        StartCoroutine(FinishTextCoroutine());
    }

    private IEnumerator FinishTextCoroutine()
    {
        yield return new WaitForSeconds(waitSecondAfterTyping);
        text.text = "";

        if(GameManagement.Instance !=null) GameManagement.Instance.ExitSlidingGame();

        else if(toClose != null)
        {
            foreach (GameObject go in toClose)
            {
                if (go != null)
                    go.SetActive(false);
            }
        }
    }

    public virtual void WriteText()
    {
        StartCoroutine(TypeText());
    }
}