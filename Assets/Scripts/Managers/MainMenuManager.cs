using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _nicknameInputField;
    
    [SerializeField] private TextMeshProUGUI _statusText, _errorText;
    
    private readonly float _dotInterval = 0.5f;
    private int _numDots = 0;
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_inputField.text))
            return;
        
        OnConnect();
        PhotonNetwork.CreateRoom(_inputField.text);
    }
    
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_inputField.text))
            return;
        
        OnConnect();
        PhotonNetwork.JoinRoom(_inputField.text);
    }
    
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
            string dots = "";
            for (int i = 0; i < _numDots; i++)
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
    
    public void GenerateNickname() => _nicknameInputField.text = GetRandomName();
    
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
    
    private IEnumerator ShowError(string message)
    {
        _errorText.text = message;
        _errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _errorText.gameObject.SetActive(false);
    }
    
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    
    public void Update()
    {
        Debug.Log($"{Screen.currentResolution.width + " x " + Screen.currentResolution.height}");
        Debug.Log($"ResolutionIndex {PlayerPrefsManager.ResolutionIndex}");
        Debug.Log($"FPSIndex {PlayerPrefsManager.FPSIndex}");
    }
}
