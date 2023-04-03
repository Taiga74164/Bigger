using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuOpener : MonoBehaviour
{
    [SerializeField] private PlayMenu _playMenu;
    
    public void OpenMenu() => _playMenu.Open();
}
