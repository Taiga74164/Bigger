using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    private static T _instance;
    public static T Instance => _instance;
    
    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
    
    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Close();
    }
    
    public virtual void Open() => gameObject.SetActive(true);
    
    public virtual void Close() => gameObject.SetActive(false);
}

[RequireComponent(typeof(Canvas))]
public abstract class Menu : Menu<Menu>
{
    public virtual void OpenMenu()
    {
        if (MenuManager.Instance != null)
            MenuManager.Instance.OpenMenu(this);
    }
    
    public virtual void CloseMenu()
    {
        if (MenuManager.Instance != null)
            MenuManager.Instance.CloseMenu();
    }
}