using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    public static float MasterVolume
    {
        get => PlayerPrefs.GetFloat("MasterVolume", 1f);
        set => PlayerPrefs.SetFloat("MasterVolume", value);
    }
    
    public static float Brightness
    {
        get => PlayerPrefs.GetFloat("Brightness", 1f);
        set => PlayerPrefs.SetFloat("Brightness", value);
    }
    
    public static bool VSync
    {
        get => PlayerPrefs.GetInt("VSync", 1) == 1;
        set => PlayerPrefs.SetInt("VSync", value ? 1 : 0);
    }
    
    public static int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex", 0);
        set => PlayerPrefs.SetInt("ResolutionIndex", value);
    }
    
    public static int WindowModeIndex
    {
        get => PlayerPrefs.GetInt("WindowModeIndex", 0);
        set => PlayerPrefs.SetInt("WindowModeIndex", value);
    }
    
    public static int FPSIndex
    {
        get => PlayerPrefs.GetInt("FPSIndex", 0);
        set => PlayerPrefs.SetInt("FPSIndex", value);
    }
    
    public static bool FirstLaunch
    {
        get => PlayerPrefs.GetInt("FirstLaunch", 1) == 1;
        set => PlayerPrefs.SetInt("FirstLaunch", value ? 1 : 0);
    }
    
    public static void Save() => PlayerPrefs.Save();
}