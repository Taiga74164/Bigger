using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Collectable interface is implemented on any object that can be 
 * "picked up" by the player. This could be coins, or power ups, etc. 
 */
interface ICollectable
{
    void Collect(GameObject _player); //interface method called when the object is collected by a player.

    public bool CanCollect { get; set; }

}
