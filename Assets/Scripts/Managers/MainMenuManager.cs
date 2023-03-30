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
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _nicknameInputField;
    
    [SerializeField] private TextMeshProUGUI _statusText, _errorText;
    
    private readonly float _dotInterval = 0.5f;
    private int _numDots = 0;

    #region Events
    
    public void Update()
    {
        //
    }

    #endregion

    #region Photon Callbacks
    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(_nicknameInputField.text) ? GetRandomName() : _nicknameInputField.text;
        StopCoroutine(DotAnimation());
        PhotonNetwork.LoadLevel("Playground");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _statusText.gameObject.SetActive(false);
        StartCoroutine(ShowError(message));
        _inputField.gameObject.SetActive(true);
    }
    
    #endregion
    
    #region Public Methods
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_inputField.text))
            return;
        
        if (DoesRoomExist(_inputField.text))
        {
            StartCoroutine(ShowError("Room already exists"));
            return;
        }
        
        OnConnect();
        
        // var roomOptions = new RoomOptions
        // {
        //     MaxPlayers = byte.Parse(_maxPlayersInputField.text),
        //     CustomRoomProperties = new Hashtable
        //     {
        //         { "GameMode", _gameModeDropdown.options[_gameModeDropdown.value].text }
        //     },
        //     CustomRoomPropertiesForLobby = new[] { "GameMode" },
        //     IsOpen = true,
        //     IsVisible = true
        // };
        
        PhotonNetwork.CreateRoom(_inputField.text);
    }
    
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_inputField.text))
            return;
        
        OnConnect();
        PhotonNetwork.JoinRoom(_inputField.text);
    }
    
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
    
    #endregion
    
    #region Private Methods
    
    private void OnConnect()
    {
        _inputField.gameObject.SetActive(false);
        _statusText.gameObject.SetActive(true);
        _errorText.gameObject.SetActive(false);
        StartCoroutine(DotAnimation());
    }    
    
    private IEnumerator DotAnimation()
    {
        while (true)
        {
            _numDots = (_numDots + 1) % 4;
            var dots = "";
            for (var i = 0; i < _numDots; i++)
            {
                dots += ".";
            }
            _statusText.text  = "Connecting" + dots;
            yield return new WaitForSeconds(_dotInterval);
        }
    }
    
    private string GetRandomName()
    {
        var names = JObject.Parse(Resources.Load<TextAsset>("JSON/nicknames").text)["nicknames"]?.ToObject<List<string>>();
        var rand = names![Random.Range(0, names.Count)];
        return rand;
    }
    
    private IEnumerator ShowError(string message)
    {
        _errorText.text = message;
        _errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _errorText.gameObject.SetActive(false);
    }
    
    private bool DoesRoomExist(string roomName)
    {
        if (LoadingSceneManager.Instance.CachedRoomList.TryGetValue(roomName, out var room))
            return !room.RemovedFromList;
        
        return false;
    }
    
    #endregion
    
}
