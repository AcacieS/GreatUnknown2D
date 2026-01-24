using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public Vector2 WorldPosition {get; private set;}
    public event Action Clicked;

    private void OnMousePosition(InputValue value)
    {
        WorldPosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }
    private void OnInteraction(InputValue _)
    {
        Clicked?.Invoke();
    }
}
