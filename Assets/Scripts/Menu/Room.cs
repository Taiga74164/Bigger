using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    public TMP_Text RoomName;
    
    public void JoinRoom() => MainMenuManager.Instance.JoinRoom(RoomName.text);
}
