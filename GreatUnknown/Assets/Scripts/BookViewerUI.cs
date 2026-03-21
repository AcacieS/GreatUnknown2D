using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class BookViewerUI : MonoBehaviour
{
    [Header("Pages (single pages in order)")]
    [SerializeField] private List<Sprite> pages = new List<Sprite>();

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
    [Header("Sound")]
    [SerializeField] private string[] soundNames;
    private int index = 0;

    private void Awake()
    {
        // Verify Connections
        if (pages == null) { Fatal("Missing nextButton Reference"); return; }
        if (leftPageImage == null) { Fatal("Missing leftPageImage Reference"); return; }
        if (rightPageImage == null) { Fatal("Missing rightPageImage Reference"); return; }
        if (prevButton == null) { Fatal("Missing prevButton Reference"); return; }
        if (nextButton == null) { Fatal("Missing nextButton Reference"); return; }
        if (escapeButton == null) { Fatal("Missing escapeButton Reference"); return; }

        // Initialize
        if (firstPageOnRight) pages.Insert(0, null);
        if (blankPageSprite != null && pages.Count % 2 == 1) pages.Add(blankPageSprite);
        navigate.action.performed += Navigate;
        escape.action.performed += (__unused_context) => Close();
        prevButton.onClick.AddListener(Prev);
        nextButton.onClick.AddListener(Next);
        escapeButton.onClick.AddListener(Close);
        Refresh();
    }

    private void Fatal(string reason)
    {
        Debug.LogError(reason);
        gameObject.SetActive(false);
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
        if (direction.x > 0)
        {
            Next();
        }
        else if (direction.x < 0)
        {
            Prev();
        }
    }
    
    private void RandomPaperSound()
    {
        int randomSoundIndex = Random.Range(0, soundNames.Length);
        SoundManager.instance.PlaySound(soundNames[randomSoundIndex]);
    }
    public void Next()
    {
	    if (CanGoNext()) {
            RandomPaperSound();
            
            index++;
        }
        Refresh();
    }
    

    public void Prev()
    {
        
        if (CanGoPrev()) {
            RandomPaperSound();
            index--;
        }
        Refresh();
    }

    private void Refresh()
    {
        var leftPageSprite = pages[index * 2];
        SetPage(leftPageImage, leftPageSprite);
        var rightPageSprite = SecondPageShown() ? pages[index * 2 + 1] : null;
        SetPage(rightPageImage, rightPageSprite);
        UpdateButtons();
    }

    private bool SecondPageShown()
    {
	return index * 2 + 1 < pages.Count;
    }

    private static void SetPage(Image img, Sprite sprite)
    {
	img.enabled = sprite != null;
        img.sprite = sprite;
    }

    private void UpdateButtons()
    {
        //Can i actually click the buttons?
        if (prevButton != null) prevButton.interactable = CanGoPrev();
        if (nextButton != null) nextButton.interactable = CanGoNext();
    }

    private bool CanGoPrev()
    {
        return pages != null && index != 0;
    }

    private bool CanGoNext()
    {
        return pages != null && (index + 1) * 2 < pages.Count;
    }
}
