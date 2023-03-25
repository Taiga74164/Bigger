using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ILiving
{
    /// <summary>
    /// Returns the living entity's attribute holder.
    /// </summary>
    /// <returns>An attribute holder.</returns>
    AttributeHolder GetAttributes();
}

/// <summary>
/// Collectable interface is implemented on any object that can be
/// picked up by the player. This could be coins, or power ups, etc.
/// </summary>
public interface ICollectable
{
    /// <summary>
    /// Interface method called when the object is collected by a player.
    /// </summary>
    /// <param name="player">The local player</param>
    public void Collect(GameObject player);
    public bool CanCollect { get; set; }

}