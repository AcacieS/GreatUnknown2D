using UnityEngine;


public class DayTransition : TypingEffect
{
    [SerializeField] private AudioInfo dayTransitionSound;
    
    public override void WriteText()
    {
        NextDay();
    }
    private void NextDay()
    {
        currentDayCanvas.SetActive(true);
        textToShow = GameManagement.Instance.GetNbDayLeft()+" DAYS FROM THE ANOMALY";
        SoundManager.instance.PlaySound(dayTransitionSound, SoundState.FadeOut);
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

}
