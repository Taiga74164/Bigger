using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages inputs globally for the game.
/// </summary>
public class InputManager : MonoBehaviour
{
    private static InputActions _actions;

    // Player actions.
    public static InputAction Move;
    public static InputAction Jump;
    public static InputAction Dash;
    public static InputAction Attack;

    // Interface actions.
    public static InputAction Cursor;

    private void Awake()
    {
        // Create the input actions asset.
        _actions = new InputActions();

        // Update the player actions.
        Move = _actions.Player.Move;
        Jump = _actions.Player.Jump;
        Dash = _actions.Player.Dash;
        Attack = _actions.Player.Attack;
        // Update the interface actions.
        Cursor = _actions.Interface.ShowCursor;
    }

    #region Boilerplate

    private void OnEnable()
    {
        // Enable the input actions.
        _actions.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions.
        _actions.Disable();
    }

    #endregion
}