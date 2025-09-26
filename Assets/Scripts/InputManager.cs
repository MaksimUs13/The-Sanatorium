using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputActions inputActions;
    public event EventHandler OnSprint;
    public event EventHandler OnStopSprint;
    private void Awake()
    {
        inputActions = new InputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Sprint.performed += Sprint;
        inputActions.Player.Sprint.canceled += StopSprinting;
    }
    private void OnDisable()
    {
        inputActions.Player.Sprint.performed -= Sprint;
        inputActions.Player.Sprint.canceled -= StopSprinting;
        inputActions.Disable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
    public Vector2 GetLookVector()
    {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }
    private void Sprint(InputAction.CallbackContext context)
    {
        OnSprint?.Invoke(this, EventArgs.Empty);
    }
    private void StopSprinting(InputAction.CallbackContext context)
    {
        OnStopSprint?.Invoke(this, EventArgs.Empty);
    }
}
