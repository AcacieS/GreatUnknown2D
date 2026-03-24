using UnityEngine;

public class OpeningTyping: TypingEffect
{
    [SerializeField] private AudioInfo firstDaySound;
    [SerializeField] private TypingEffect currentDayTransition;
    private AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        textToShow = text.text;
        SoundManager.instance.PlaySound(firstDaySound, SoundState.None, PlaySoundState.Play, audioSource);
        text.text = "";
        WriteText();
    }
    public override void FinishText()
    {
        if (Input.anyKeyDown)
        {
            if(text.text == textToShow)
            {
                SoundManager.instance.FadeOut(1f, audioSource);
                currentDayCanvas.SetActive(false);
                if (currentDayTransition != null) currentDayTransition.WriteText();
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
    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    
    
    
}
