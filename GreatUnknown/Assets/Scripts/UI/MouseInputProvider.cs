using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public Vector2 WorldPosition {get; private set;}
    public event Action Clicked;
    public event Action Released;

    private void OnMousePosition(InputValue value)
    {
        WorldPosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }
    private void OnInteraction(InputValue value)
    {
        if (value.isPressed)
        {
            Clicked?.Invoke();
        }
        else
        {
            Released?.Invoke();
        }
    }
}
