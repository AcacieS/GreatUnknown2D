using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FaxMachine : MonoBehaviour, IClickable
{
    [Header("Data")]
    [SerializeField] private FaxCatalog catalog;
    [SerializeField] private FaxState state;

    [Header("Visuals")]
    [SerializeField] private FaxOverlayUI faxOverlayUI;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite blinkSpriteA;
    [SerializeField] private Sprite blinkSpriteB;
    [SerializeField, Min(0.02f)] private float blinkIntervalSeconds = 0.35f;

    private SpriteRenderer _sr;
    private Coroutine _blinkRoutine;
    private bool _isBlinking;

    private void Awake()
    {
        if (faxOverlayUI == null) Ext.WarnRefAndDisable("faxOverlayUI", this);
        state.Clear();
        _sr = GetComponent<SpriteRenderer>();
        if (idleSprite != null) _sr.sprite = idleSprite;
    }

    /// <summary>
    /// Phase 1 entry point:
    /// 1) NotifyFax() -> blinking
    /// 2) Resolve sprite -> push into FaxLog
    /// </summary>
    public void NewFaxMessage(string messageId)
    {
        NotifyFax();

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

        state.AddMessage(sprite);
    }

    public void NotifyFax()
    {
        if (_isBlinking) return;

        if (_sr == null) _sr = GetComponent<SpriteRenderer>();

        if (blinkSpriteA == null || blinkSpriteB == null)
        {
            Debug.LogWarning("[FaxMachine] Blink sprites not set. NotifyFax() will do nothing.");
            return;
        }

        _isBlinking = true;
        _blinkRoutine = StartCoroutine(BlinkLoop());
    }

    public void StopNotifyFax()
    {
        _isBlinking = false;

        if (_blinkRoutine != null)
        {
            StopCoroutine(_blinkRoutine);
            _blinkRoutine = null;
        }

        if (idleSprite != null && _sr != null)
            _sr.sprite = idleSprite;
    }

    private IEnumerator BlinkLoop()
    {
        bool toggle = false;

        while (_isBlinking)
        {
            _sr.sprite = toggle ? blinkSpriteA : blinkSpriteB;
            toggle = !toggle;
            yield return new WaitForSeconds(blinkIntervalSeconds);
        }
    }

    /// <summary>
    /// Your ClickHandler will call this automatically when the fax is clicked. :contentReference[oaicite:1]{index=1}
    /// Phase 1 behavior: stop blinking + mark read (UI display comes later).
    /// </summary>
    public void OnClick()
    {
        if(state.GetNbFax()==0) return;
        StopNotifyFax();

        if (state != null)
            state.MarkAllRead();

        faxOverlayUI.Open();
    }

    // Quick manual test from Inspector
    [ContextMenu("Debug/Add Test Fax (id = TEST)")]
    private void DebugAddTestFax()
    {
        NewFaxMessage("TEST");
    }
}