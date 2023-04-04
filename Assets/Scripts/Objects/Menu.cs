using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    public virtual void Open() => gameObject.SetActive(true);
    
    public virtual void Close() => gameObject.SetActive(false);
}

[RequireComponent(typeof(Canvas))]
public abstract class Menu : Menu<Menu>
{
    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            CloseMenu();
    }
    
    public virtual void OpenMenu() => MenuManager.Instance.OpenMenu(this);
    
    public virtual void CloseMenu() => MenuManager.Instance.CloseMenu();
}