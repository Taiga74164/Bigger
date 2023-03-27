using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents a resolution option to fix the issue when opening the settings menu.
/// </summary>
public sealed class ResolutionOption
{
    public string Option;
    public int Width;
    public int Height;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="option">Option name in ${width x height} format.</param>
    /// <param name="width">Screen width.</param>
    /// <param name="height">Screen height.</param>
    public ResolutionOption(string option, int width, int height)
    {
        this.Option = option;
        this.Width = width;
        this.Height = height;
    }
}