using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Newtonsoft.Json.Linq;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _nicknameInputField;

    [SerializeField] private GameObject _statusText, _errorText;
    
    private float _dotInterval = 0.5f;
    private float _timer = 0f;
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
        _statusText.SetActive(true);
        _errorText.SetActive(false);
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
            _statusText.GetComponent<TextMeshProUGUI>().text  = "Connecting" + dots;
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
        StartCoroutine(ShowError());
        _inputField.gameObject.SetActive(true);
    }
    
    private IEnumerator ShowError()
    {
        _errorText.SetActive(true);
        yield return new WaitForSeconds(2);
        _errorText.SetActive(false);
    }
    
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
