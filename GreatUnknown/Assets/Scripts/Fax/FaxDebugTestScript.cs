using UnityEngine;

public class FaxDebugTester : MonoBehaviour
{
    [SerializeField] private FaxMachine faxMachine;
    [SerializeField] private FaxOverlayUI faxOverlayUI;

    private void Awake()
    {
        if (faxMachine == null)
            faxMachine = FindFirstObjectByType<FaxMachine>();

        if (faxOverlayUI == null)
            faxOverlayUI = FindFirstObjectByType<FaxOverlayUI>();
    }

    [ContextMenu("Test Fax / Day1 Morning 0 / Open Then Close")]
    public void TestDay1Morning0OpenThenClose()
    {
        if (faxMachine == null)
        {
            Debug.LogError("FaxMachine reference missing.");
            return;
        }

        faxMachine.NewFaxMessage("day1_morning");

        // Simulate opening the fax UI
        if (faxOverlayUI != null)
            faxOverlayUI.Open();

        // Simulate closing it
        if (faxOverlayUI != null)
            faxOverlayUI.Close();

        Debug.Log("Test complete: pushed day1_morning_0, opened fax UI, then closed it.");
    }

    [ContextMenu("Test Fax / Day2 + Day3")]
    public void TestDay2AndDay3()
    {
        if (faxMachine == null)
        {
            Debug.LogError("FaxMachine reference missing.");
            return;
        }

        faxMachine.NewFaxMessage("day2_morning");
        faxMachine.NewFaxMessage("day3_morning");

        Debug.Log("Test complete: pushed day2_morning and day3_morning_0.");
    }
}