using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject RoomPrefab;
    public Transform Content;
    
    private int _previousRoomCount;
    
    private void Update()
    {
        UpdateRoomList();
        foreach (var a in LoadingSceneManager.Instance.CachedRoomList.Values)
        {
            Debug.Log("room name "+ a.Name + ", max players " + a.MaxPlayers.ToString());
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateRoomList()
    {
        // Since calling this function every Update is expensive, We compare the previous and current room counts.
        if (LoadingSceneManager.Instance.CachedRoomList.Count == _previousRoomCount)
            return;
        
        // Update the previous room count.
        _previousRoomCount = LoadingSceneManager.Instance.CachedRoomList.Count;
        
        // Destroy any existing room objects.
        foreach (var child in Content.GetComponentsInChildren<Transform>())
        {
            if (child == Content)
                continue;
            
            Destroy(child.gameObject);
        }
        
        // Instantiate a new room object for each room in the list.
        foreach (var room in LoadingSceneManager.Instance.CachedRoomList.Values)
        {
            if (room.IsOpen && room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                // Instantiate a new room object.
                var newRoom = Instantiate(RoomPrefab, Content);
                
                // Set the room name.
                newRoom.GetComponentInChildren<Room>().RoomName.SetText(room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")");
                
                // Add a listener to the button to join the room.
                newRoom.GetComponent<Button>().onClick.AddListener(() => MainMenuManager.Instance.JoinRoom(room));
            }
        }
    }
}
