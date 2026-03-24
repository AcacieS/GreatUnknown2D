using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FaxMachine : MonoBehaviour, IClickable
{
    [Header("Data")]
    [SerializeField]
    private FaxCatalog catalog;

    [SerializeField]
    private FaxState state;

    [Header("Visuals")]
    [SerializeField]
    private FaxOverlayUI faxOverlayUI;

    private Animator _anim;

    private void Awake()
    {
        if (catalog == null)
        {
            Ext.WarnRefAndDisable("catalog", this);
            return;
        }
        if (state == null)
        {
            Ext.WarnRefAndDisable("state", this);
            return;
        }
        if (faxOverlayUI == null)
        {
            Ext.WarnRefAndDisable("faxOverlayUI", this);
            return;
        }
        state.Clear();
        _anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _anim.SetBool("Notify", state.UnreadCount != 0);
    }
    
    public void SendStartingFaxMessages(int day)
    {
        foreach (string id in catalog.GetStartingOnDay(day))
        {
            NewFaxMessage(id);
        }
    }

    /// <summary>
    /// Phase 1 entry point:
    /// 1) NotifyFax() -> blinking
    /// 2) Resolve sprite -> push into FaxLog
    /// </summary>
    public void NewFaxMessage(string messageId)
    {
        if (catalog == null)
        {
            Debug.LogError("[FaxMachine] Missing FaxCatalog reference.");
            return;
        }
        if (state == null)
        {
            Debug.LogError("[FaxMachine] Missing FaxState reference.");
            return;
        }

        if (!catalog.TryGet(messageId, out var sprite) || sprite == null)
        {
            Debug.LogError($"[FaxMachine] Unknown message id '{messageId}'. Add it to FaxCatalog.");
            return;
        }

        // Check if the message could be added successfully
        // Adding may fail if the message has already been sent.
        if (!state.AddMessage(sprite))
            return;

        NotifyFax();
    }

    public void NotifyFax()
    {
        _anim.SetBool("Notify", true);
        FaxMachineSound();
    }

    public void StopNotifyFax()
    {
        _anim.SetBool("Notify", false);
    }

    public void ClearAllFaxMessages()
    {
        if (state == null)
            return;

        state.Clear();
        StopNotifyFax();
    }

    /// <summary>
    /// Your ClickHandler will call this automatically when the fax is clicked. :contentReference[oaicite:1]{index=1}
    /// Phase 1 behavior: stop blinking + mark read (UI display comes later).
    /// </summary>
    public void OnClick()
    {
        if (state.GetNbFax() == 0)
            return;
        StopNotifyFax();

        if (state == null)
            return;
        FaxOpenSound();
        state.MarkAllRead();
        faxOverlayUI.Open();
    }

    private void FaxMachineSound() => SoundManager.instance.PlaySound("faxMachine");

    private void FaxOpenSound() => SoundManager.instance.PlaySound("faxOpen");

    // Quick manual test from Inspector
    [ContextMenu("Debug/Add Test Fax (id = TEST)")]
    private void DebugAddTestFax()
    {
        NewFaxMessage("TEST");
    }
}
