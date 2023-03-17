using System.Drawing;
using TMPro;
using UnityEngine;

/// <summary>
/// An entity-specific attribute holder.
/// </summary>
public class EntityAttributeHolder : AttributeHolder
{
    #region Component Reference

    [Header("Components")]
    public TMP_Text NameText;
    public TMP_Text SizeText;
    
    #endregion

    #region Entity Attributes

    [Header("Attributes")]
    public Attribute Name = new(Attribute.NameAttribute, "Entity");
    public Attribute Size = new(Attribute.SizeAttribute, 1.0f);

    #endregion

    /// <summary>
    /// Applies the attributes to the holder.
    /// </summary>
    protected void ApplyAttributes()
    {
        // Apply references.
        NameText.text = Name.GetValue() as string;
        SizeText.text = Size.GetValue() as string;
        
        // Add attributes.
        CreateAttribute(Name);
        CreateAttribute(Size);
    }

    private void Awake() => ApplyAttributes();
}