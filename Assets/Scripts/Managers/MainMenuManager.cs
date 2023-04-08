using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Newtonsoft.Json.Linq;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] private TMP_InputField _nicknameInputField;
    
    #region Events
    
    private void Start()
    {
        // Set the cursor to be visible.
        Cursor.lockState = CursorLockMode.None;
        // Show the cursor from view.
        Cursor.visible = true;
    }

    #endregion
    
    #region Photon Callbacks
    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(_nicknameInputField.text) ? GetRandomName() : _nicknameInputField.text;
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name} as {PhotonNetwork.NickName}");
        // StopCoroutine(DotAnimation());
        PhotonNetwork.LoadLevel("Playground");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room. {message}");
    }
    
    #endregion
    
    #region Methods
    
    // public void JoinRoom()
    // {
    //     if (string.IsNullOrEmpty(_inputField.text))
    //         return;
    //     
    //     OnConnect();
    //     PhotonNetwork.JoinRoom(_inputField.text);
    // }
    
    public void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);
    
    public void JoinRoom(RoomInfo room) => PhotonNetwork.JoinRoom(room.Name);
    
    public void GenerateNickname() => _nicknameInputField.text = GetRandomName();
    
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    
    private string GetRandomName()
    {
        var names = JObject.Parse(Resources.Load<TextAsset>("JSON/nicknames").text)["nicknames"]?.ToObject<List<string>>();
        var rand = names![Random.Range(0, names.Count)];
        return rand;
    }
    
    #endregion
    
}
