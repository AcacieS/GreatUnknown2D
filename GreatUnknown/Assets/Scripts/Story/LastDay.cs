using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastDay : MonoBehaviour
{
    public static LastDay Instance {get; private set;}
    [SerializeField] private int klaxonDelaySeconds = 30;
    [SerializeField] private int promptDelaySeconds = 5;
    [SerializeField] private AudioInfo Klaxon;
    [SerializeField] private AudioInfo LastDayAmbiance;
    [SerializeField] private GameObject LastDayTerminal;
    [SerializeField] private GameObject LastDayPromptPrefab;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        //SoundManager.instance.PlaySound(LastDayAmbiance);
        //StartCoroutine(DelayKlaxonSound());
    }

    private IEnumerator DelayKlaxonSound()
    {
        yield return new WaitForSeconds(klaxonDelaySeconds);

        Debug.Log("Playing the Klaxon Sound");
        SoundManager.instance.PlaySound(Klaxon);

        yield return new WaitForSeconds(promptDelaySeconds);

        var l = Instantiate(LastDayPromptPrefab);
        l.transform.SetParent(LastDayTerminal.gameObject.transform.parent);
        l.GetComponent<Button>().onClick.AddListener(FinalTerminalScene);
    }

    public void FinalTerminalScene()
    {
        LastDayTerminal.SetActive(true);
    }
}
