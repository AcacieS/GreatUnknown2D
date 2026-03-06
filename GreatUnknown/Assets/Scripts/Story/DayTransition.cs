using UnityEngine;


public class DayTransition : TypingEffect
{
    [SerializeField] private AudioInfo dayTransitionSound;
    [SerializeField] private AudioSource audioSource;
    public override void WriteText()
    {
        NextDay();
    }
    public override void Awake()
    {
        base.Awake();
    }
    private void NextDay()
    {
        currentDayCanvas.SetActive(true);
        textToShow = GameManagement.Instance.GetNbDayLeft()+" DAYS FROM THE ANOMALY";
        audioSource.PlayOneShot(dayTransitionSound.soundClip);
        //SoundManager.instance.PlaySound(dayTransitionSound, SoundState.FadeOut);
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

}
