using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardList : MonoBehaviour
{
    public GameObject LeaderboardItemPrefab;
    public Transform Content;
    private Dictionary<int, GameObject> _leaderboardItems = new Dictionary<int, GameObject>();
    
    private void Start()
    {
        // Instantiate the Prefab for each player.
        foreach (var playerData in GameManager.Instance.PlayerDataDict)
        {
            var listItem = Instantiate(LeaderboardItemPrefab, Content);
            _leaderboardItems.Add(playerData.Key, listItem);
        }
    }
    
    private void Update()
    {
        UpdateLeaderboardList();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateLeaderboardList()
    {
        // Sort the PlayerData dictionary by PlayerSize in descending order.
        var sortedPlayerData = GameManager.Instance.PlayerDataDict.OrderByDescending(x => x.Value.PlayerSize);
        
        // Clear the existing leaderboard items. May cause performance issues.
        foreach (var leaderboardItem in _leaderboardItems)
            Destroy(leaderboardItem.Value);
        _leaderboardItems.Clear();
        
        // Update the values for each player in the leaderboard, sorted by player size.
        var rank = 1;
        foreach (var playerData in sortedPlayerData)
        {
            // Instantiate a new Prefab for each player in the sorted player data.
            var listItem = Instantiate(LeaderboardItemPrefab, Content);
            _leaderboardItems.Add(playerData.Key, listItem);
            
            // Get the LeaderboardItem component and set the player name.
            var leaderboardItem = listItem.GetComponent<LeaderboardItem>();
            leaderboardItem.SetPlayerName($"{rank}. {playerData.Value.PlayerName}");
            
            // Increment the rank for the next player.
            rank++;
        }
    }
}
