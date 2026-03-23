using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BookViewerUI : MonoBehaviour
{
    [Header("Pages (single pages in order)")]
    [FormerlySerializedAs("pages")]
    [SerializeField] private CodexPages codexPages;

    [Header("UI Actions")]
    [SerializeField] private InputActionReference navigate;
    [SerializeField] private InputActionReference escape;

    [Header("UI References")]
    [SerializeField] private Image pageImage;
    //[SerializeField] private Image leftPageImage;
    //[SerializeField] private Image rightPageImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button escapeButton;

    [Header("Behavior (hover for details)")]
    [Tooltip("If true: first page is shown on the RIGHT (left is blank). If false: first page is shown on the LEFT.")]
    [SerializeField] private bool firstPageOnRight = true;
    [SerializeField] private bool hasFirstPage = true;
    [Tooltip("If true: first page is shown on the RIGHT (left is blank). If false: first page is shown on the LEFT.")]
    [SerializeField] private bool lastPageOnLeft = true;
    [SerializeField] private bool hasLastPage = true;
    private int index = 0;

    private void Awake()
    {
        // Do null checks on Awake
        if (codexPages == null) { Ext.WarnRefAndDisable("codexPages", this); return; }
        if (pageImage == null) { Ext.WarnRefAndDisable("pageImage", this); return; }
        if (prevButton == null) { Ext.WarnRefAndDisable("prevButton", this); return; }
        if (nextButton == null) { Ext.WarnRefAndDisable("nextButton", this); return; }
        if (escapeButton == null) { Ext.WarnRefAndDisable("escapeButton", this); return; }
    }

    public void Start()
    {
        Refresh();
    }

    public void OnEnable()
    {
        navigate.action.performed += Navigate;
        escape.action.performed += Close;
        prevButton.onClick.AddListener(Prev);
        nextButton.onClick.AddListener(Next);
        escapeButton.onClick.AddListener(Close);
    }

    public void OnDisable()
    {
        navigate.action.performed -= Navigate;
        escape.action.performed -= Close;
        prevButton.onClick.RemoveListener(Prev);
        nextButton.onClick.RemoveListener(Next);
        escapeButton.onClick.RemoveListener(Close);
    }

    public void Open() { gameObject.SetActive(true); RandomPaperSound(); }
    public void Close() { RandomPaperSound(); gameObject.SetActive(false); }
    public void Close(InputAction.CallbackContext context) => Close();

    public void Navigate(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x < 0) Prev(); else
        if (context.ReadValue<Vector2>().x > 0) Next();
    }

    public void Next() { 
        if (CanGoNext()) 
        { 
            RandomPaperSound();
            index++;
            Refresh(); 
        } 
    }
    public void Prev() { if (CanGoPrev()) { RandomPaperSound(); index--; Refresh(); } }

    private void RandomPaperSound() => SoundManager.instance.PlaySound("paper" + Random.Range(1, 5));

    private void Refresh()
    {
        Debug.Log("index: "+index);
        SetPage(pageImage, codexPages.pageSprites[index]);
        // SetPage(leftPageImage, pageSprites[index * 2]);
        // SetPage(rightPageImage, pageSprites[index * 2 + 1]);
        prevButton.interactable = CanGoPrev();
        nextButton.interactable = CanGoNext();
    }

    private static void SetPage(Image img, Sprite sprite)
    {
	    img.enabled = sprite != null;
        img.sprite = sprite;
    }

    private bool CanGoPrev() => codexPages.pageSprites != null && index != 0;
    private bool CanGoNext() => codexPages.pageSprites != null && (index + 1) < codexPages.pageSprites.Count;
}
