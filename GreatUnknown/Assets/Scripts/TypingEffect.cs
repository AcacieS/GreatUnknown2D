using UnityEngine;
using TMPro;
using System.Collections;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingEffect : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string textToShow;
    [SerializeField] private GameObject currentDayCanvas;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float waitSecondAfterTyping = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (textToShow == null || textToShow == "")
        {
            textToShow = text.text;
        }
        text.text = "";
        if(currentDayCanvas == null)
        {
            currentDayCanvas = transform.parent.gameObject;
        }
        StartCoroutine(TypeText());
    }
    public IEnumerator TypeText()
    {
        text.text = "";
        foreach (char c in textToShow)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(waitSecondAfterTyping);
        text.text = "";
        currentDayCanvas.SetActive(false);
    }
    public void NextDay()
    {
        currentDayCanvas.SetActive(true);
        textToShow = GameManagement.Instance.GetNbDayLeft()+" Day left ...";
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
