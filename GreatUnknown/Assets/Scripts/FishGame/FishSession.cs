using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Fish/Fish Session")]
public class FishSession : ScriptableObject
{
    [field: SerializeField] public int Correct { get; private set; }
    [field: SerializeField] public int Wrong { get; private set; }

    public event Action Changed;

    public void ResetSession()
    {
        Correct = 0;
        Wrong = 0;
        Changed?.Invoke();
    }

    public void AddCorrect()
    {
        Correct++;
        Changed?.Invoke();
        Debug.Log("Correct now: " + Correct);
    }

    public void AddWrong()
    {
        Wrong++;
        Changed?.Invoke();
        Debug.Log("Wrong now: " + Wrong);
    }
}
