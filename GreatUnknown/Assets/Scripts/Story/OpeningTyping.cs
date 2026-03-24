using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class OpeningTyping: TypingEffect, TWTWControls.ICutsceneActions
{
    [SerializeField] private TypingEffect currentDayTransition;
    private AudioSource audioSource;
    private TWTWControls controlMaps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        SoundManager.instance.PlaySound("openingStory", SoundState.None, PlaySoundState.Play, audioSource);

        textToShow = text.text;
        text.text = "";
        WriteText();
    }

    public override void FinishText()
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

    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();

        controlMaps = new TWTWControls();
        controlMaps.Cutscene.AddCallbacks(this);
    
        if (currentDayTransition == null) { Ext.WarnRefAndDisable("currentDayTransition", this); return; }
    }

    #region Action Bindings for ICutsceneActions

    public void OnSkip(InputAction.CallbackContext context) => FinishText();

    void OnDestroy()
    {
        controlMaps.Dispose();
    }

    void OnEnable()
    {
        controlMaps.Cutscene.Enable();
    }

    void OnDisable()
    {
        controlMaps.Cutscene.Disable();
    }

    #endregion Action Bindings for ICutsceneActions
}
