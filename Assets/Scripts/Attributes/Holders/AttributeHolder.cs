using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for behavior of an attribute holder.
/// </summary>
public class AttributeHolder : MonoBehaviour
{
    /// <summary>
    /// Holder of attribute instances.
    /// </summary>
    private readonly Dictionary<string, Attribute> _attributes = new();

    /// <summary>
    /// Adds an attribute to the holder.
    /// </summary>
    /// <param name="attribute">The attribute instance.</param>
    public void CreateAttribute(Attribute attribute) => _attributes.Add(attribute.GetId(), attribute);

    /// <summary>
    /// Returns the attribute with the given ID.
    /// </summary>
    /// <param name="id">The ID of the attribute.</param>
    /// <returns>An attribute instance.</returns>
    public Attribute GetAttribute(string id) => _attributes[id];

    /// <summary>
    /// Returns the collection of attribute IDs.
    /// </summary>
    /// <returns>An array of attribute keys.</returns>
    public string[] GetAttributes() => _attributes.Keys.ToArray();

    private void Update()
    {
        // Call the update method on the attributes.
        foreach (var attribute in _attributes.Values)
            attribute.Update();
    }
}