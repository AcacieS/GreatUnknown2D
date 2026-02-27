using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookViewerUI : MonoBehaviour
{
    [Header("Pages (single pages in order)")]
    [SerializeField] private List<Sprite> pages = new List<Sprite>();

    [Header("UI References")]
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [Header("Optional UI")]
    [SerializeField] private TextMeshProUGUI pageText;     // optional
    [SerializeField] private Sprite blankPageSprite;       // optional fallback if you prefer blanks instead of disabling

    [Header("Behavior")]
    [Tooltip("If true: first page is shown on the RIGHT (left is blank). If false: first page is shown on the LEFT.")]
    [SerializeField] private bool firstPageOnRight = true;

    // This is the "left page index" for the current spread.
    // Example: 0 means we're at the beginning (special-cased single page).
    // In normal spreads, leftIndex is 1, 3, 5, ... (and right is leftIndex+1).
    private int leftIndex = 0;

    private void Awake()
    {
        // Wire buttons (you can also wire via Inspector; this is safe and convenient)
        if (prevButton != null) prevButton.onClick.AddListener(Prev);
        if (nextButton != null) nextButton.onClick.AddListener(Next);

        // Initialize
        ClampLeftIndex();
        Refresh();
    }

    /// <summary>
    /// Go to the next spread (typically +2 pages).
    /// </summary>
    public void Next()
    {
        if (!CanGoNext()) return;

        // Special case: from start single page to first spread
        if (leftIndex == 0)
        {
            // After showing page 0 alone, the next view should start the first spread.
            // We choose leftIndex = 1 so spread becomes (1,2).
            leftIndex = 1;
        }
        else
        {
            leftIndex += 2;
        }

        ClampLeftIndex();
        Refresh();
    }

    /// <summary>
    /// Go to the previous spread (typically -2 pages).
    /// </summary>
    public void Prev()
    {
        if (!CanGoPrev()) return;

        // If we are at the first spread (1,2), going back returns to the single first page (0).
        if (leftIndex == 1)
        {
            leftIndex = 0;
        }
        else
        {
            leftIndex -= 2;
        }

        ClampLeftIndex();
        Refresh();
    }

    private void Refresh()
    {
        // No pages: show nothing and disable navigation
        if (pages == null || pages.Count == 0)
        {
            SetPage(leftPageImage, null, false);
            SetPage(rightPageImage, null, false);
            UpdateButtons();
            UpdatePageText();
            return;
        }

        int last = pages.Count - 1;

        // Case A: Start single page (index 0 only)
        if (leftIndex == 0)
        {
            if (firstPageOnRight)
            {
                // left blank, right shows page 0
                //Remember blankPageSprite is a FallBack!
                SetPage(leftPageImage, blankPageSprite, blankPageSprite != null);
                SetPage(rightPageImage, pages[0], true);
            }
            else
            {
                // left shows page 0, right blank
                SetPage(leftPageImage, pages[0], true);
                SetPage(rightPageImage, blankPageSprite, blankPageSprite != null);
            }

            UpdateButtons();
            UpdatePageText(singleShownIndex: 0);
            return;
        }

        // Case B: End single page (if leftIndex points to last page)
        // Example: pages.Count is even => last index is odd; you will eventually land on leftIndex == last
        // Example: pages.Count is odd => last index is even; you can still land on leftIndex == last as needed
        if (leftIndex >= last)
        {
            // show only the last page on left, and blank/disable right
            SetPage(leftPageImage, pages[last], true);
            SetPage(rightPageImage, blankPageSprite, blankPageSprite != null);
            UpdateButtons();
            UpdatePageText(singleShownIndex: last);
            return;
        }

        // Case C: Normal spread (leftIndex, leftIndex+1)
        int rightIndex = leftIndex + 1;

        SetPage(leftPageImage, pages[leftIndex], true);

        if (rightIndex <= last)
            SetPage(rightPageImage, pages[rightIndex], true);
        else
            SetPage(rightPageImage, blankPageSprite, blankPageSprite != null);

        UpdateButtons();
        UpdatePageText(spreadLeftIndex: leftIndex, spreadRightIndex: rightIndex <= last ? rightIndex : (int?)null);
    }

    private void SetPage(Image img, Sprite sprite, bool enabled)
    {
        if (img == null) return;
        img.sprite = sprite;
        img.enabled = enabled;
    }

    private void UpdateButtons()
    {
        //Can i actually click the buttons?
        if (prevButton != null) prevButton.interactable = CanGoPrev();
        if (nextButton != null) nextButton.interactable = CanGoNext();
    }

    private bool CanGoPrev()
    {
        // At the very start single page, you cannot go back.
        return pages != null && pages.Count > 0 && leftIndex != 0;
    }

    private bool CanGoNext()
    {
        if (pages == null || pages.Count == 0) return false;

        int last = pages.Count - 1;

        // From start (0) you can go next if there exists at least page 1
        if (leftIndex == 0)
            return pages.Count >= 2;

        // If leftIndex is already at or beyond last, no next
        if (leftIndex >= last) return false;

        // Otherwise yes
        return true;
    }

    private void ClampLeftIndex()
    {
        if (pages == null || pages.Count == 0)
        {
            leftIndex = 0;
            return;
        }

        int last = pages.Count - 1;

        // Keep 0 as a valid special state
        if (leftIndex == 0) return;

        // Ensure leftIndex is not negative
        if (leftIndex < 0) leftIndex = 0;

        // If beyond last, clamp to last (end single page state)
        if (leftIndex > last) leftIndex = last;
    }

    private void UpdatePageText(int? singleShownIndex = null, int? spreadLeftIndex = null, int? spreadRightIndex = null)
    {
        if (pageText == null) return;
        if (pages == null || pages.Count == 0)
        {
            pageText.text = "";
            return;
        }

        // Display 1-based page numbers for humans
        if (singleShownIndex.HasValue)
        {
            int p = singleShownIndex.Value + 1;
            pageText.text = $"Page {p} / {pages.Count}";
            return;
        }

        if (spreadLeftIndex.HasValue)
        {
            int l = spreadLeftIndex.Value + 1;
            if (spreadRightIndex.HasValue)
            {
                int r = spreadRightIndex.Value + 1;
                pageText.text = $"Pages {l}-{r} / {pages.Count}";
            }
            else
            {
                pageText.text = $"Page {l} / {pages.Count}";
            }
            return;
        }

        pageText.text = "";
    }
}
