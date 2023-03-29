using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<int, PlayerData> _playerDataDict = new Dictionary<int, PlayerData>();
    
    #region Events
    
    private void Update()
    {
        foreach (var a in _playerDataDict)
        {
            Debug.Log($"up: {a.Key} {a.Value}");
        }
        
        // DebugPlayerData();
    }
    
    #endregion
    
    #region Photon Callbacks
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("PlayerData"))
            return;
        
        // Get the updated player data.
        var bytes = (byte[]) changedProps["PlayerData"];
        var playerData = PlayerData.Parser.ParseFrom(bytes);
        
        // Update the player data dictionary.
        _playerDataDict[targetPlayer.ActorNumber] = playerData;
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Get all player data.
        _playerDataDict.Remove(otherPlayer.ActorNumber);
    }
    
    #endregion

    private void DebugPlayerData()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // Check if the player data dictionary already contains this player's data.
            if (!player.CustomProperties.ContainsKey("PlayerData"))
                continue;
            
            // Get the updated player data.
            var bytes = (byte[]) player.CustomProperties["PlayerData"];
            var playerData = PlayerData.Parser.ParseFrom(bytes);
            
            Debug.Log($"Player {playerData.PlayerName} has size {playerData.PlayerSize}");
            
            // Add the player data to the dictionary.
            // _playerDataDict.Add(player.ActorNumber, playerData);
        }
    }
}
