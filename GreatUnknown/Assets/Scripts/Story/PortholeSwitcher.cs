using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortholeSwitcher : MonoBehaviour
{
    [SerializeField] private Transform lastDayPortholePos;
    [SerializeField] private List<Porthole> portholes;

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnDaySwitch(int day)
    {
        var selected = portholes[0];

        foreach (var candidate in portholes)
            if (selected.day > day || (candidate.day > selected.day && candidate.day <= day))
                selected = candidate;

        sprite.sprite = selected.sprite;
        if (day == 5) 
        {
            // Don't sway the sub on day 6
            sprite.GetComponent<Animator>().enabled = false;
            sprite.transform.position = lastDayPortholePos.position;
        }
    }
}
