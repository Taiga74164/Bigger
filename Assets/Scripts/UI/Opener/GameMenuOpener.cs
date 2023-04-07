using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuOpener : MonoBehaviour
{
    [SerializeField] private GameMenu _gameMenu;
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !_gameMenu.IsOpen)
            OpenGameMenu();
    }
    
    public void OpenGameMenu() => _gameMenu.OpenMenu();
}
