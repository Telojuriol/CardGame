using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{

    public delegate void OnTapPerformed(Vector2 tapPosition);
    public delegate void OnHoldStarted(Vector2 holdPosition);
    public delegate void OnHoldEnded();

    public static event OnTapPerformed onTapPerformed;
    public static event OnHoldStarted onHoldStarted;
    public static event OnHoldEnded onHoldEnded;

    private PlayerInput playerInput;

    private InputAction tapPositionAction;
    private InputAction tapPressAction;
    private InputAction holdPressAction;
    private InputAction holdPositionAction;

    private bool touchHolded = false;

    public static InputManager _instance;

    private void Awake()
    {
        _instance = this;
        playerInput = GetComponent<PlayerInput>();

        //Tap Actions
        tapPressAction = playerInput.actions["TapPress"];
        tapPositionAction = playerInput.actions["TapPosition"];

        //Hold Actions
        holdPressAction = playerInput.actions["Hold"];
        holdPositionAction = playerInput.actions["HoldPosition"];
    }

    private void OnEnable()
    {
        tapPressAction.performed += TapPressed;
        holdPressAction.performed += HoldStarted;
        holdPressAction.canceled += HoldEnded;
    }

    private void OnDisable()
    {
        tapPressAction.performed -= TapPressed;
        holdPressAction.performed -= HoldStarted;
        holdPressAction.canceled -= HoldEnded;
    }

    private void TapPressed(InputAction.CallbackContext context)
    {
        onTapPerformed?.Invoke(Camera.main.ScreenToWorldPoint(_instance.tapPositionAction.ReadValue<Vector2>()));
    }

    private void HoldStarted(InputAction.CallbackContext context)
    {
        touchHolded = true;
        onHoldStarted?.Invoke(Camera.main.ScreenToWorldPoint(_instance.holdPositionAction.ReadValue<Vector2>()));
    }

    private void HoldEnded(InputAction.CallbackContext context)
    {        
        if (touchHolded)
        {
            onHoldEnded?.Invoke();
        }
        touchHolded = false;
    }

    public static bool IsScreenHolded()
    {
        return _instance.touchHolded;
    }

    public static Vector2 GetHoldPosition()
    {
        return Camera.main.ScreenToWorldPoint(_instance.holdPositionAction.ReadValue<Vector2>());
    }
}
