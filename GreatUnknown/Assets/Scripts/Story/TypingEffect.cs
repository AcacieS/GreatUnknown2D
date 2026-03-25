using UnityEngine;
using TMPro;
using System.Collections;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingEffect : MonoBehaviour
{
    protected TextMeshProUGUI text;
    protected string textToShow;
    [SerializeField] protected GameObject currentDayCanvas;
    [SerializeField] protected float typingSpeed = 0.05f;
    [SerializeField] protected float waitSecondAfterTyping = 3f;
    public virtual void Start()
    {
        
    }
    public virtual void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(currentDayCanvas == null)
        {
            currentDayCanvas = transform.parent.gameObject;
        }
    }
    public IEnumerator TypeText()
    {
        text.text = "";
        foreach (char c in textToShow)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        if (GetType() != typeof(OpeningTyping)) FinishText();
    }
    public virtual void FinishText()
    {
        StartCoroutine(FinishTextCoroutine());
    }
    private IEnumerator FinishTextCoroutine()
    {
        yield return new WaitForSeconds(waitSecondAfterTyping);
        text.text = "";
        currentDayCanvas.SetActive(false);
        GameManagement.Instance.SpecialEventDay();
    }
    public virtual void WriteText()
    {
        StartCoroutine(TypeText());
    }
    public virtual void Update()
    {
        
    }
    public void SkipText()
    {
        StopAllCoroutines();
        text.text = textToShow;
    }
}
