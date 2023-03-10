using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSettingsMenu : MonoBehaviour
{
    [SerializeField] private SettingsMenu _settingsMenuPrefab;
    
    public void OpenMenu()
    {
        _settingsMenuPrefab.Open();
    }
}
