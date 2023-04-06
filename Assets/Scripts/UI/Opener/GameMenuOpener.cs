using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuOpener : Singleton<GameMenuOpener>
{
    [SerializeField] private GameMenu _gameMenu;
    
    public void OpenGameMenu() => _gameMenu.OpenMenu();
}
