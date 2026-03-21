using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BookViewerUI : MonoBehaviour
{
    [Header("Pages (single pages in order)")]
    [FormerlySerializedAs("pages")]
    [SerializeField] private List<Sprite> pageSprites = new List<Sprite>();

    [Header("UI Actions")]
    [SerializeField] private InputActionReference navigate;
    [SerializeField] private InputActionReference escape;

    [Header("UI References")]
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button escapeButton;

    [Header("Optional UI")]
    [SerializeField] private Sprite blankPageSprite;

    [Header("Behavior (hover for details)")]
    [Tooltip("If true: first page is shown on the RIGHT (left is blank). If false: first page is shown on the LEFT.")]
    [SerializeField] private bool firstPageOnRight = true;
    [Tooltip("If true: first page is shown on the RIGHT (left is blank). If false: first page is shown on the LEFT.")]
    [SerializeField] private bool lastPageOnLeft = true;
    private int index = 0;

    private void Awake()
    {
        // Do null checks on Awake
        if (pageSprites == null) { Ext.Fatal("Missing pageSprites Reference", this); return; }
        if (leftPageImage == null) { Ext.Fatal("Missing leftPageImage Reference", this); return; }
        if (rightPageImage == null) { Ext.Fatal("Missing rightPageImage Reference", this); return; }
        if (prevButton == null) { Ext.Fatal("Missing prevButton Reference", this); return; }
        if (nextButton == null) { Ext.Fatal("Missing nextButton Reference", this); return; }
        if (escapeButton == null) { Ext.Fatal("Missing escapeButton Reference", this); return; }
        if (blankPageSprite == null) { Ext.Warning("Missing blankPageSprite Reference", this); blankPageSprite = null; }

        // Reformat pages list (must have an even size, null's are for invisible pages)
        if (firstPageOnRight) pageSprites.Insert(0, null);
        if (lastPageOnLeft && pageSprites.Count % 2 == 0) pageSprites.Insert(pageSprites.Count - 1, blankPageSprite);
        if (pageSprites.Count % 2 == 1) pageSprites.Add(blankPageSprite);

        // Listen for Events
        navigate.action.performed += Navigate;
        escape.action.performed += (__unused_context) => Close();
        prevButton.onClick.AddListener(Prev);
        nextButton.onClick.AddListener(Next);
        escapeButton.onClick.AddListener(Close);
        Refresh();
    }

    public void Open()
    {
	    gameObject.SetActive(true);
    }

    public void Close()
    {
	    gameObject.SetActive(false);
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf) return;

        var direction = context.ReadValue<Vector2>();
        if (direction.x > 0) Next(); else if (direction.x < 0) Prev();
    }

    public void Next()
    {
	    if (CanGoNext()) { RandomPaperSound(); index++; Refresh(); }
    }

    public void Prev()
    {
        if (CanGoPrev()) { RandomPaperSound(); index--; Refresh(); }
    }

    private void RandomPaperSound()
    {
        SoundManager.instance.PlaySound("paper" + Random.Range(1, 5));
    }

    private void Refresh()
    {
        SetPage(leftPageImage, pageSprites[index * 2]);
        SetPage(rightPageImage, pageSprites[index * 2 + 1]);
        if (prevButton != null) prevButton.interactable = CanGoPrev();
        if (nextButton != null) nextButton.interactable = CanGoNext();
    }

    private static void SetPage(Image img, Sprite sprite)
    {
	    img.enabled = sprite != null;
        img.sprite = sprite;
    }

    private bool CanGoPrev() => pageSprites != null && index != 0;
    private bool CanGoNext() => pageSprites != null && (index + 1) * 2 < pageSprites.Count;
}
