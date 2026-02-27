using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Fax/Fax State")]
public class FaxState : ScriptableObject
{
    private List<FaxPrintJob> pending = new();
    private List<FaxPrintJob> printed = new();

    public IReadOnlyList<FaxPrintJob> Pending => pending;
    public IReadOnlyList<FaxPrintJob> Printed => printed;

    public event Action NewPending;
    public event Action NewPrinted;

    private void OnEnable()
    {
        pending.Clear();
        printed.Clear();
    }

    public void AddPending(FaxPrintJob newJob)
    {
        pending.Add(newJob);
        NewPending?.Invoke();
        Debug.Log("Added Pending Job: " + newJob.Header);
    }

    public void PrintPending()
    {
        if (pending.Count == 0)
        {
            Debug.LogWarning("PrintPending() called with none pending");
            return;
        }

        FaxPrintJob job = pending[0];

        printed.Add(job);
        pending.RemoveAt(0);

        NewPrinted?.Invoke();
        Debug.Log("Finished Printing Job: " + job.Header);
    }
}