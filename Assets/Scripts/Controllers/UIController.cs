using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class UIController : MonoBehaviour
{
    private InputAction _showCursor;
    public CinemachineInputProvider cameraInputProvider;
    
    private void Start()
    {
        // Setup input action references.
        _showCursor = InputManager.Cursor;
        
        // Initialize the UI.
        CaptureCursor();
    }
    
    private void Update()
    {
        UpdateCursorStatus();
    }
    
    private void CaptureCursor()
    {
        // Lock the cursor to the game.
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor from view.
        Cursor.visible = false;
    }
    
    private void UpdateCursorStatus()
    {
        // Check the cursor input action or if the game is paused.
        var cursor = GameManager.Instance.IsGamePaused || _showCursor.ReadValue<float>() > 0;
        
        // Update the cursor properties.
        Cursor.visible = cursor;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        
        // Update the camera input provider.
        cameraInputProvider.enabled = !cursor;
    }
}
