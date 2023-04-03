using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostMenuOpener : MonoBehaviour
{
    [SerializeField] private HostMenu _hostMenu;
    
    public void OpenMenu() => _hostMenu.Open();
}
