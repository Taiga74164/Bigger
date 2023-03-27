using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu<SettingsMenu>
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Toggle _vSyncToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _windowModeDropdown;
    [SerializeField] private TMP_Dropdown _fpsDropdown;
    
    private float _masterVolume;
    private float _brightness;
    private bool _vSync;
    private int _resolutionIndex;
    private int _windowModeIndex;
    private int _fpsIndex;

    private Resolution[] _resolutions;
    private HashSet<ResolutionOption> _resolutionOptions;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Get all available resolutions
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        
        // Create a set to avoid duplicates
        _resolutionOptions = new HashSet<ResolutionOption>();
        
        // Find the current resolution and set it as the default
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + " x " + _resolutions[i].height;
            if (_resolutionOptions.Any(x => x.Option.Contains(option)))
                continue;
            
            // Add the option to the set
            _resolutionOptions.Add(new ResolutionOption(option, _resolutions[i].width, _resolutions[i].height));
            
            // Add the option to the dropdown
            _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }
        _resolutionDropdown.SetValueWithoutNotify(PlayerPrefsManager.ResolutionIndex);
        
        LoadSettings();
        UpdateUIElements();
        UpdateSettings();
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Close();
        
        // for (var i = 0; i < _resolutionOptions.Count; i++)
        // {
        //     var option = _resolutionOptions.ToList()[i];
        //     Debug.Log($"Count: {i}: {option.Width}, {option.Height}");
        // }
    }
    
    /// <summary>
    /// Adjusts the master volume.
    /// </summary>
    /// <param name="value">Volume value from 0 to 10.</param>
    public void SetVolume(float value)
    {
        _masterVolume = value;
        // ToDo: Add all audio sources
        SaveSettings();
    }
    
    /// <summary>
    /// Adjusts the brightness.
    /// </summary>
    /// <param name="value">Brightness value from 0 to 1.</param>
    public void SetBrightness(float value)
    {
        _brightness = value;
        Screen.brightness = _brightness;
        SaveSettings();
    }
    
    /// <summary>
    /// Sets the VSync.
    /// </summary>
    /// <param name="value"></param>
    public void SetVSync(bool value)
    {
        _vSync = value;
        QualitySettings.vSyncCount = _vSync ? 1 : 0;
        SaveSettings();
    }
    
    /// <summary>
    /// Sets the resolution.
    /// </summary>
    /// <param name="index">The index position of the chosen resolution</param>
    public void SetResolution(int index)
    {
        _resolutionIndex = index;
        Screen.SetResolution(_resolutionOptions.ToList()[index].Width, _resolutionOptions.ToList()[index].Height, Screen.fullScreenMode);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets the FPS.
    /// </summary>
    /// <param name="index">The index position of the chosen FPS value</param>
    public void SetFPS(int index)
    {
        _fpsIndex = index;
        Application.targetFrameRate = int.Parse(_fpsDropdown.options[index].text);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets the window mode.
    /// </summary>
    /// <param name="index">The index position of the chosen Window Mode</param>
    public void SetWindowMode(int index)
    {
        _windowModeIndex = index;
        switch (_windowModeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        SaveSettings();
    }
    
    private void LoadSettings()
    {
        _masterVolume = PlayerPrefsManager.MasterVolume;
        _brightness = PlayerPrefsManager.Brightness;
        _vSync = PlayerPrefsManager.VSync;
        _resolutionIndex = PlayerPrefsManager.ResolutionIndex;
        _windowModeIndex = PlayerPrefsManager.WindowModeIndex;
        _fpsIndex = PlayerPrefsManager.FPSIndex;
    }
    
    private void UpdateUIElements()
    {
        _masterVolumeSlider.value = _masterVolume;
        _brightnessSlider.value = _brightness;
        _vSyncToggle.isOn = _vSync;
        _resolutionDropdown.value = _resolutionIndex;
        _windowModeDropdown.value = _windowModeIndex;
        _fpsDropdown.value = _fpsIndex;
    }
    
    private void UpdateSettings()
    {
        SetVolume(_masterVolume);
        SetBrightness(_brightness);
        SetVSync(_vSync);
        SetResolution(_resolutionIndex);
        SetWindowMode(_windowModeIndex);
        SetFPS(_fpsIndex);
    }
    
    private void SaveSettings()
    {
        PlayerPrefsManager.MasterVolume = _masterVolume;
        PlayerPrefsManager.Brightness = _brightness;
        PlayerPrefsManager.VSync = _vSync;
        PlayerPrefsManager.ResolutionIndex = _resolutionIndex;
        PlayerPrefsManager.WindowModeIndex = _windowModeIndex;
        PlayerPrefsManager.FPSIndex = _fpsIndex;
        PlayerPrefsManager.Save();
    }
    
    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
    }
    
    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }
}
