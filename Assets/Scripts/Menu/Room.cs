using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Room : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomName;
    
    public void JoinRoom() => MainMenuManager.Instance.JoinRoom(RoomName.text);
}
