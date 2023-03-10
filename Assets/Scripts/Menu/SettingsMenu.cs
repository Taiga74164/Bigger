using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu<SettingsMenu>
{
    [SerializeField] private Slider _masterVolumeSlider;
    // [SerializeField] private Slider _musicVolumeSlider;
    // [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Toggle _vSyncToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _windowModeDropdown;
    // [SerializeField] private Dropdown _textureQualityDropdown;
    // [SerializeField] private Dropdown _antiAliasingDropdown;
    // [SerializeField] private Dropdown _shadowQualityDropdown;
    // [SerializeField] private Dropdown _vSyncDropdown;
    [SerializeField] private TMP_Dropdown _fpsDropdown;
    
    private float _masterVolume;
    // private float _musicVolume;
    // private float _sfxVolume;
    private float _brightness;
    private bool _vSync;
    private int _resolutionIndex;
    private int _windowModeIndex;
    // private int _textureQuality;
    // private int _antiAliasing;
    // private int _shadowQuality;
    // private int _vSyncIndex;
    private int _fpsIndex;

    private Resolution[] _resolutions;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Get all available resolutions
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        
        // Load settings
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        _brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        _vSync = PlayerPrefs.GetInt("VSync", 1) == 1;
        _resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        _windowModeIndex = PlayerPrefs.GetInt("WindowModeIndex", 0);
        _fpsIndex = PlayerPrefs.GetInt("FPSIndex", 0);
        
        UpdateUIElements();
        UpdateSettings();
    }
    
    /// <summary>
    /// Adjusts the master volume.
    /// </summary>
    /// <param name="value">Volume value from 0 - 10.</param>
    public void SetVolume(float value)
    {
        _masterVolume = value;
        // ToDo: Add all audio sources
        SaveSettings();
    }
    
    /// <summary>
    /// Adjusts the brightness.
    /// </summary>
    /// <param name="value">Brightness value from 0 - 1.</param>
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
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreenMode);
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
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        SaveSettings();
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
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("Brightness", _brightness);
        PlayerPrefs.SetInt("VSync", _vSync ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionIndex", _resolutionIndex);
        PlayerPrefs.SetInt("WindowModeIndex", _windowModeIndex);
        PlayerPrefs.SetInt("FPSIndex", _fpsIndex);
        PlayerPrefs.Save();
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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Close();
    }
}
