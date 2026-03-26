using UnityEngine;


public class DayTransition : TypingEffect
{
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
        GameObject submarine = submarineDays[GameManagement.Instance.GetNbDayPassed()];
        submarine.SetActive(true);
        if (GameManagement.Instance.GetNbDayLeft() == 0)
            textToShow = "THE ANOMALY";
        else if (GameManagement.Instance.GetNbDayLeft() == 1)
            textToShow = "1 DAY FROM THE ANOMALY";
        else
            textToShow = GameManagement.Instance.GetNbDayLeft() + " DAYS FROM THE ANOMALY";
        SoundManager.instance.PlaySound("dayTransition", SoundState.None, PlaySoundState.Play, audioSource);
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }
}
