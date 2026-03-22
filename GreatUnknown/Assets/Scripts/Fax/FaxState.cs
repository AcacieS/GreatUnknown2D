using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fax/Fax State")]
public class FaxState : ScriptableObject
{
    [field: SerializeField] public int UnreadCount { get; private set; }

    // Oldest -> newest
    [SerializeField] private List<Sprite> faxLog = new List<Sprite>();
    public IReadOnlyList<Sprite> FaxLog => faxLog;

    public event Action Changed;
    public event Action NewUnread;

    public void AddMessage(Sprite messageSprite)
    {
        if (messageSprite == null) return;

        faxLog.Add(messageSprite);
        UnreadCount++;
        NewUnread?.Invoke();
        Changed?.Invoke();
    }

    public void MarkAllRead()
    {
        if (UnreadCount == 0) return;
        UnreadCount = 0;
        Changed?.Invoke();
        Debug.Log("[FaxState] Marked all read.");
    }

    [ContextMenu("Debug/Clear Log")]
    public void Clear()
    {
        faxLog.Clear();
        UnreadCount = 0;
        Changed?.Invoke();
    }
}