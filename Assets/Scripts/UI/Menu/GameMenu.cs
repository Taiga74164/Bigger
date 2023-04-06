using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMenu : Menu
{
    public void OnEnable() => GameManager.Instance.IsGamePaused = true;
    
    public void OnDisable() => GameManager.Instance.IsGamePaused = false;
    
    protected override void Update() => base.Update();
    
    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void ResumeGame() => CloseMenu();
}
