using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance => _instance;
    
    private Stack<Menu> _menuStack = new Stack<Menu>();
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    /// <summary>
    /// Opens a specific menu and closes the previous menu.
    /// </summary>
    /// <param name="menu">The menu you want to close. This menu must inherit the Menu class</param>
    public void OpenMenu(Menu menu)
    {
        if (menu?.gameObject == null)
            return;

        if (_menuStack.Count > 0)
        {
            // If the menu is already open, do nothing.
            if (_menuStack.TryPeek(out var topMenu) && topMenu == menu)
                return;

            // Close the previous menu.
            topMenu.Close();
        }
        
        // Open the menu.
        menu.Open();
        
        // Add the menu to the stack.
        _menuStack.Push(menu);
    }
    
    /// <summary>
    /// Closes the top menu and opens the previous menu.
    /// </summary>
    public void CloseMenu()
    {
        if (_menuStack.Count == 0)
            return;
        
        // Close the top menu.
        var topMenu = _menuStack.Pop();
        topMenu.Close();
        
        if (_menuStack.Count > 0)
        {
            // Open the previous menu.
            var nextMenu = _menuStack.Peek();
            nextMenu.Open();
        }
    }
}
