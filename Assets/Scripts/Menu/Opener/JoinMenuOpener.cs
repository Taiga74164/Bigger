using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinMenuOpener : MonoBehaviour
{
    [SerializeField] private JoinMenu _joinMenu;
    
    public void OpenMenu() => _joinMenu.Open();
}
