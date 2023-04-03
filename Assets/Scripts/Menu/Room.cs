using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Room : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomName;
    
    /// <summary>
    /// Since we added player count to the room name in the RoomList script, we need to trim it before joining the room.
    /// </summary>
    public void JoinRoom() => MainMenuManager.Instance.JoinRoom(RoomName.text.IndexOf(' ', StringComparison.OrdinalIgnoreCase) > 0 ? RoomName.text.Substring(0, RoomName.text.IndexOf(' ', StringComparison.OrdinalIgnoreCase)).Trim() : RoomName.text);
}
