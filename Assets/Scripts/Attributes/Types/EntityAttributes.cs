using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Generic attribute for an entity's name.
/// </summary>
[Serializable]
public class NameAttribute : Attribute
{
    public NameAttribute(string baseValue)
        : base(Attributes.Name, baseValue) { }
}

/// <summary>
/// Generic attribute for entity's size.
/// </summary>
[Serializable]
public class SizeAttribute : Attribute
{
    public SizeAttribute(float baseValue)
        : base(Attributes.Size, baseValue) { }
}