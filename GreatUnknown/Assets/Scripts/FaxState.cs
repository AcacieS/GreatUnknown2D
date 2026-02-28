using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Fax/Fax State")]
public class FaxState : ScriptableObject
{
    private List<FaxPrintJob> pending = new List<FaxPrintJob>();
    public List<FaxPrintJob> printed = new List<FaxPrintJob>();

    public event Action NewPending;
    public event Action NewPrinted;

    public void AddPending(FaxPrintJob newJob)
    {
        pending.Add(newJob);
        NewPending?.Invoke();
        Debug.Log("Added Pending Job: " + newJob.header);
    }

    public void PrintPending()
    {
        if (pending.Count == 0)
        {
            Debug.Warn("PrintPending() called with none pending");
            return;
        }
        printed.Add(pending[0]);
        pending.RemoveAt(0);
        NewPrinted?.Invoke();
        Debug.Log("Finished Printing Job: " + printed.Last());
    }
}
