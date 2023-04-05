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
    private float _winningCondition;
    private bool _gameOver;
    
    private Dictionary<int, PlayerData> _playerDataDict = new Dictionary<int, PlayerData>();
    
    #region Events
    
    private void Start() => _winningCondition = float.Parse(PhotonNetwork.CurrentRoom.CustomProperties["MaxScore"].ToString());

    private void Update()
    {
        if (!_gameOver)
            return;
        
        // ToDo:
        // - Show game over screen.
        // - Reset the game or return to the main menu.
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
        
        
        if (playerData.PlayerSize >= _winningCondition)
        {
            Debug.Log($"Player {playerData.PlayerName} has won!");
            _gameOver = true;
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Remove the player data from the dictionary.
        _playerDataDict.Remove(otherPlayer.ActorNumber);
    }
    
    #endregion

    // private void GetAllPlayerData()
    // {
    //     foreach (var player in PhotonNetwork.PlayerList)
    //     {
    //         // Check if the player data dictionary already contains this player's data.
    //         if (_playerDataDict.ContainsKey(player.ActorNumber))
    //             continue;
    //         
    //         // Get the updated player data.
    //         var bytes = (byte[]) player.CustomProperties["PlayerData"];
    //         var playerData = PlayerData.Parser.ParseFrom(bytes);
    //         
    //         // Debug.Log($"Player {playerData.PlayerName} has size {playerData.PlayerSize}");
    //         
    //         // Add the player data to the dictionary.
    //         _playerDataDict.Add(player.ActorNumber, playerData);
    //     }
    // }
}
