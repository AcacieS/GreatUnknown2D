using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FaxViewerUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FaxState state;

    [Header("UI")]
    [SerializeField] private RectTransform centerContainer;
    [SerializeField] private RectTransform sideContainer;
    [SerializeField] private Image pagePrefab;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [Header("Layout")]
    [SerializeField] private int centerDepth = 4;
    [SerializeField] private Vector2 centerBehindOffset = new Vector2(18f, -6f);
    [SerializeField] private Vector2 sideOffset = new Vector2(-14f, -5f);

    [Header("UI Actions")]
    [SerializeField] private InputActionReference navigate;
    [SerializeField] private InputActionReference escape;

    private readonly List<Image> spawned = new();
    private int currentIndex;

    void Awake()
    {
        if (state == null) { Ext.WarnRefAndDisable("state", this); return; }
        if (centerContainer == null) { Ext.WarnRefAndDisable("centerContainer", this); return; }
        if (sideContainer == null) { Ext.WarnRefAndDisable("sideContainer", this); return; }
        if (pagePrefab == null) { Ext.WarnRefAndDisable("pagePrefab", this); return; }
        if (leftButton == null) { Ext.WarnRefAndDisable("leftButton", this); return; }
        if (rightButton == null) { Ext.WarnRefAndDisable("rightButton", this); return; }
        if (centerBehindOffset == null) { Ext.WarnRefAndDisable("centerBehindOffset", this); return; }
        if (sideOffset == null) { Ext.WarnRefAndDisable("sideOffset", this); return; }
        if (navigate == null) { Ext.WarnRefAndDisable("navigate", this); return; }
        if (escape == null) { Ext.WarnRefAndDisable("escape", this); return; }
    }

    private void OnEnable()
    {
        // start on latest
        if (state != null && state.FaxLog.Count > 0)
            currentIndex = state.FaxLog.Count - 1;

        Render();
        navigate.action.performed += Navigate;
        escape.action.performed += Close;
    }

    private void OnDisable()
    {
        navigate.action.performed += Navigate;
        escape.action.performed += Close;
        RandomPaperSound();
    }
    
    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy) return;
        if (context.ReadValue<Vector2>().x < 0) GoNewer(); else
        if (context.ReadValue<Vector2>().x > 0) GoOlder();
    }

    public void Close(InputAction.CallbackContext context) => gameObject.transform.parent.gameObject.SetActive(false);

    public void GoOlder() // Left arrow
    {
        if (state == null) return;
        if (currentIndex <= 0) return;
        RandomPaperSound();

        currentIndex--;
        Render();
    }

    public void GoNewer() // Right arrow
    {
        if (state == null) return;
        if (currentIndex >= state.FaxLog.Count - 1) return;
        RandomPaperSound();

        currentIndex++;
        Render();
    }

    private void RandomPaperSound() => SoundManager.instance.PlaySound("paper" + Random.Range(1, 5));

   private void Render()
    {
        ClearSpawned();
        if (state == null) return;

        var log = state.FaxLog;
        if (log.Count == 0) return;

        // CENTER STACK: oldest behind, current on top
        int firstCenterIndex = Mathf.Max(0, currentIndex - centerDepth);
        int layers = 0;

        for (int i = firstCenterIndex; i <= currentIndex; i++, layers++)
        {
            var img = SpawnPage(centerContainer, log[i]);
            img.rectTransform.anchoredPosition = centerBehindOffset * (currentIndex - i);
            img.transform.SetAsLastSibling();
        }

        // SIDE STACK: newer pages, with nearest newer on top or reverse depending on what you want
        int sideLayer = 0;
        for (int i = log.Count - 1; i > currentIndex; i--, sideLayer++)
        {
            var img = SpawnPage(sideContainer, log[i]);
            img.rectTransform.anchoredPosition = sideOffset * sideLayer;
            img.transform.SetAsLastSibling();
        }

        leftButton.interactable = currentIndex > 0;
        rightButton.interactable = currentIndex < state.FaxLog.Count - 1;
    }
    

    private Image SpawnPage(RectTransform parent, Sprite sprite)
    {
        var img = Instantiate(pagePrefab, parent);
        img.sprite = sprite;
        spawned.Add(img);
        return img;
    }

    private void ClearSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null) Destroy(spawned[i].gameObject);
        }
        spawned.Clear();
    }
}
