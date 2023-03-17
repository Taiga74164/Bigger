using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        InitializeResolution();
    }
    
    private void InitializeResolution()
    {
        if (PlayerPrefsManager.FirstLaunch)
        {
            PlayerPrefsManager.ResolutionIndex = GetMatchingResolutionIndex();
            PlayerPrefsManager.Save();
            PlayerPrefsManager.FirstLaunch = false;
        }

        // var resolutionIndex = PlayerPrefsManager.ResolutionIndex;
        // var resolutions = Screen.resolutions;
        // if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        // {
        //     Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreenMode);
        // }
    }
    
    private int GetMatchingResolutionIndex()
    {
        var resolutions = Screen.resolutions;
        var currentResolution = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
        
        HashSet<string> options = new HashSet<string>();
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            var option = resolutions[i].width + " x " + resolutions[i].height;
            if (options.Contains(option))
                continue;
            
            options.Add(option);

            var list = options.ToList();
            if (option == currentResolution)
                return list.IndexOf(option);
        }
        
        return 0;
    }
    
    void Start() => PhotonNetwork.ConnectUsingSettings();
    
    public override void OnConnectedToMaster()
    {
        // Do stuff here. Can insert a loading screen, Game Title, etc.
        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby() => SceneManager.LoadScene("MainMenu");
}
