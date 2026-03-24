using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundCatalog", menuName = "Scriptable Objects/SoundCatalog")]
public class SoundCatalog : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public string id;     // e.g. "Day1_LetterA"
        public AudioInfo audioInfo; // the letter sprite
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    // Runtime cache (built lazily)
    private Dictionary<string, AudioInfo> _map; 

    [ContextMenu("Build Map of Sound")]
    public void BuildMapIfNeeded()
    {
        if (_map != null) return;

        _map = new Dictionary<string, AudioInfo>(StringComparer.Ordinal);
        foreach (var e in entries)
        {
            if (string.IsNullOrWhiteSpace(e.id) || e.audioInfo == null) continue;
            _map[e.id] = e.audioInfo; // last one wins if duplicates
        }
    }
    [ContextMenu("Refresh Dictionary")]
    public void RefreshDictionary()
    {
        _map = new Dictionary<string, AudioInfo>(StringComparer.Ordinal);
        foreach (var e in entries)
        {
            if (string.IsNullOrWhiteSpace(e.id) || e.audioInfo == null) continue;
            _map[e.id] = e.audioInfo; // last one wins if duplicates
        }
    }

    [ContextMenu("Update Map of Sound")]
    public void AddNewElements()
    {
        if (_map == null)
        {
            BuildMapIfNeeded();
        }

        foreach (var e in entries)
        {
            if (!_map.ContainsKey(e.id))
            {
                _map[e.id] = e.audioInfo;
            }
        }
    }

    public bool TryGet(string id, out AudioInfo audioInfo)
    {
        BuildMapIfNeeded();
        return _map.TryGetValue(id, out audioInfo);
    }
}
