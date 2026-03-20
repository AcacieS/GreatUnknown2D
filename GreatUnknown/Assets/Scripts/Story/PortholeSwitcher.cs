using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortholeSwitcher : MonoBehaviour
{
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
    }
}
