using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class HostMenu : Menu<HostMenu>
{
    [SerializeField] private TMP_InputField _roomNameField;
    [SerializeField] private TMP_Dropdown _maxPlayersDropdown;
    [SerializeField] private TMP_Dropdown _maxScoreDropdown;
    [SerializeField] private TextMeshProUGUI _statusText, _errorText;
    
    private readonly float _dotInterval = 0.5f;
    private int _numDots = 0;
    
    protected override void Update() => base.Update();
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _statusText.gameObject.SetActive(false);
        StartCoroutine(ShowError(message));
        _roomNameField.gameObject.SetActive(true);
    }
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameField.text))
            return;
        
        if (DoesRoomExist(_roomNameField.text))
        {
            StartCoroutine(ShowError("Room already exists"));
            return;
        }
        
        OnConnect();
        
        var roomOptions = new RoomOptions
        {
            MaxPlayers = byte.Parse(_maxPlayersDropdown.value.ToString()),
            CustomRoomProperties = new Hashtable
            {
                { "MaxScore", _maxScoreDropdown.options[_maxScoreDropdown.value].text }
            },
            CustomRoomPropertiesForLobby = new[] { "MaxScore" },
            IsOpen = true,
            IsVisible = true
        };
        
        PhotonNetwork.CreateRoom(_roomNameField.text);
    }
    
    private void OnConnect()
    {
        _roomNameField.gameObject.SetActive(false);
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
    
}
