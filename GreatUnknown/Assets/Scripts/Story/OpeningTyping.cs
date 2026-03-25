using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(AudioSource))]
public class OpeningTyping: TypingEffect
{
    [SerializeField] private TypingEffect currentDayTransition;
    private AudioSource audioSource;
    private IDisposable controlCallback = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        SoundManager.instance.PlaySound("openingStory", SoundState.None, PlaySoundState.Play, audioSource);

        textToShow = text.text;
        text.text = "";
        WriteText();
        controlCallback = InputSystem.onAnyButtonPress.Call(FinishText);
    }

    public void FinishText(InputControl unusedControl) => FinishText();
    public override void FinishText()
    {
        if(text.text == textToShow)
        {
            SoundManager.instance.FadeOut(1f, audioSource);
            if (currentDayTransition != null) currentDayTransition.WriteText();
            currentDayCanvas.SetActive(false);
            if (controlCallback != null) controlCallback.Dispose();
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

        if (currentDayTransition == null) { Ext.WarnRefAndDisable("currentDayTransition", this); return; }
    }
}
