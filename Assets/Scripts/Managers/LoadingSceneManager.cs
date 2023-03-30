using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    [HideInInspector]
    public Dictionary<string, RoomInfo> CachedRoomList = new Dictionary<string, RoomInfo>();
    
    #region Events
    
    private void Awake()
    {
        InitializeResolutionIndex();
        
        // Make sure this object is not destroyed when loading a new scene because we need the cached room list.
        DontDestroyOnLoad(this);
    }
    
    private void Start() => PhotonNetwork.ConnectUsingSettings();
    
    #endregion
    
    #region Photon Callbacks
    
    public override void OnConnectedToMaster()
    {
        // Do stuff here. Can insert a loading screen, Game Title, etc.
        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("MainMenu");
        
        // Destroy all children of this object to prevent UI from appearing in the next scene.
        foreach (var child in transform.GetComponentsInChildren<Transform>())
            if (child != transform)
                Destroy(child.gameObject);
    }
    
    /// <summary>
    /// https://doc.photonengine.com/pun/v2/lobby-and-matchmaking/matchmaking-and-lobby#default_lobby_type
    /// </summary>
    /// <param name="roomList">List of available rooms.</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList) => UpdateCachedRoomList(roomList);
    
    public override void OnLeftLobby() => CachedRoomList.Clear();
    
    public override void OnDisconnected(DisconnectCause cause) => CachedRoomList.Clear();
    
    #endregion
    
    #region Methods
    
    private void InitializeResolutionIndex()
    {
        if (!PlayerPrefsManager.FirstLaunch)
            return;
        
        PlayerPrefsManager.ResolutionIndex = GetMatchingResolutionIndex();
        PlayerPrefsManager.Save();
        PlayerPrefsManager.FirstLaunch = false;
    }
    
    private int GetMatchingResolutionIndex()
    {
        var resolutions = Screen.resolutions;
        var currentResolution = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
        
        HashSet<string> options = new HashSet<string>();
        
        foreach (var t in resolutions)
        {
            var option = t.width + " x " + t.height;
            if (options.Contains(option))
                continue;
            
            options.Add(option);

            var list = options.ToList();
            if (option == currentResolution)
                return list.IndexOf(option);
        }
        
        return 0;
    }
    
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
            if (room.RemovedFromList)
                CachedRoomList.Remove(room.Name);
            else
                CachedRoomList[room.Name] = room;
    }
    
    #endregion
}
