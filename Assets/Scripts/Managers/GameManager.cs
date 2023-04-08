using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Singleton<GameManager>
{
    private float _winningCondition;
    private bool _gameOver;
    public bool IsGameOver => _gameOver;
    
    public Dictionary<int, PlayerData> PlayerDataDict = new Dictionary<int, PlayerData>();
    
    // private bool _isMultiplayer;
    private bool _isGamePaused;
    public bool IsGamePaused
    {
        get => _isGamePaused;
        set => _isGamePaused = value;
    }
    
    private PlayerData _winner;
    public PlayerData Winner => _winner;
    [SerializeField] private GameObject _gameOverScreen;
    
    #region Events
    
    private void Start() => _winningCondition = float.Parse(PhotonNetwork.CurrentRoom.CustomProperties["MaxScore"].ToString());

    private void Update()
    {
        if (!_gameOver)
            return;
        
        _gameOverScreen.SetActive(true);
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
        PlayerDataDict[targetPlayer.ActorNumber] = playerData;
        
        if (playerData.PlayerSize >= _winningCondition && !_gameOver && _winner == null)
        {
            Debug.Log($"Player {playerData.PlayerName} has won!");
            _winner = playerData;
            _gameOver = true;
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Remove the player data from the dictionary.
        PlayerDataDict.Remove(otherPlayer.ActorNumber);
    }
    
    #endregion

    #region Methods
    
    

    #endregion
}
