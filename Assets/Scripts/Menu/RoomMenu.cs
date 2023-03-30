using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomMenu : Menu<RoomMenu>
{
    public TMP_InputField RoomNameInputField;
    
    protected override void Update() => base.Update();
    
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(RoomNameInputField.text))
            return;
        
        MainMenuManager.Instance.JoinRoom(RoomNameInputField.text);
    }
}
