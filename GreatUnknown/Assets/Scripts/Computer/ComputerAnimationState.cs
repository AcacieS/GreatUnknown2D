using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ComputerAnimationState : MonoBehaviour
{
    [SerializeField]
    Animator computerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (computerAnimator == null)
            computerAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        bool isComputerOpen;
        if (GameManagement.Instance != null)
            isComputerOpen = false;
        else
            isComputerOpen = GameManagement.Instance.IsFishGameFinished;

        computerAnimator.SetBool("Open", isComputerOpen);
    }

    // Update is called once per frame
}
