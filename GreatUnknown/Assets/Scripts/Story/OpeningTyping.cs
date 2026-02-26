using UnityEngine;

public class OpeningTyping: TypingEffect
{
    [SerializeField] private AudioInfo firstDaySound;
    [SerializeField] private TypingEffect currentDayTransition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        textToShow = text.text;
        SoundManager.instance.PlaySound(firstDaySound);
        text.text = "";
        WriteText();
    }
    public override void FinishText()
    {
        if (Input.anyKeyDown)
        {
            if(text.text == textToShow)
            {
                currentDayCanvas.SetActive(false);
                currentDayTransition.WriteText();
            }
            else
            {
                SkipText();
            }
            
        }
    }
    public override void Update()
    {
        FinishText();
    }
    
    
    
}
