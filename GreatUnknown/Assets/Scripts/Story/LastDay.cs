using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDay : MonoBehaviour
{
    public static LastDay Instance {get; private set;}
    [SerializeField] private int klaxonDelaySeconds = 30;
    [SerializeField] private AudioInfo Klaxon;
    [SerializeField] private GameObject LastDayTerminal;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameManagement.Instance.GetNbDayLeft() == 0)
        {
            StartCoroutine(DelayKlaxonSound());
        }
    }

    private IEnumerator DelayKlaxonSound()
    {
        yield return new WaitForSeconds(klaxonDelaySeconds);

        Debug.Log("Playing the Klaxon Sound");
        SoundManager.instance.PlaySound(Klaxon);
    }

    public void ItsTheFinalCountdown()
    {
        LastDayTerminal.SetActive(true);
    }
}
