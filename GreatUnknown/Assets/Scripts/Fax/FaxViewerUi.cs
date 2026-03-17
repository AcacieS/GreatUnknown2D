using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaxViewerUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FaxState state;

    [Header("UI")]
    [SerializeField] private RectTransform centerContainer;
    [SerializeField] private RectTransform sideContainer;
    [SerializeField] private Image pagePrefab; // prefab with Image

    [Header("Layout")]
    [SerializeField] private int centerDepth = 4;
    [SerializeField] private Vector2 centerBehindOffset = new Vector2(18f, -6f);
    [SerializeField] private Vector2 sideOffset = new Vector2(-14f, -5f);

    private readonly List<Image> spawned = new();
    private int currentIndex;

    private void OnEnable()
    {
        // start on latest
        if (state != null && state.FaxLog.Count > 0)
            currentIndex = state.FaxLog.Count - 1;

        Render();
    }

    public void GoOlder() // Left arrow
    {
        if (state == null) return;
        if (currentIndex <= 0) return;

        currentIndex--;
        Render();
    }

    public void GoNewer() // Right arrow
    {
        if (state == null) return;
        if (currentIndex >= state.FaxLog.Count - 1) return;

        currentIndex++;
        Render();
    }

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