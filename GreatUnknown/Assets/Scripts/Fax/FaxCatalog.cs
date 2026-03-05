using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fax/Fax Catalog")]
public class FaxCatalog : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public string id;     // e.g. "Day1_LetterA"
        public Sprite sprite; // the letter sprite
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    // Runtime cache (built lazily)
    private Dictionary<string, Sprite> _map;

    private void BuildMapIfNeeded()
    {
        if (_map != null) return;

        _map = new Dictionary<string, Sprite>(StringComparer.Ordinal);
        foreach (var e in entries)
        {
            if (string.IsNullOrWhiteSpace(e.id) || e.sprite == null) continue;
            _map[e.id] = e.sprite; // last one wins if duplicates
        }
    }

    public bool TryGet(string id, out Sprite sprite)
    {
        BuildMapIfNeeded();
        return _map.TryGetValue(id, out sprite);
    }
}