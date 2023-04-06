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
    private SortedList<float, string> _leaderboardList = new SortedList<float, string>();
    
    private int _previousPlayerCount;
    
    private void Update()
    {
        UpdateLeaderboardList();
    }
    
    private void UpdateLeaderboardList()
    {
        if (GameManager.Instance.PlayerDataDict.Count == _previousPlayerCount)
            return;
        
        _previousPlayerCount = GameManager.Instance.PlayerDataDict.Count;
        
        // Clear the leaderboard list.
        _leaderboardList.Clear();
        
        // Loop through the player data dictionary and add the player data to the leaderboard list.
        foreach (var player in GameManager.Instance.PlayerDataDict)
        {
            // The key is the player size plus a small value to prevent duplicate keys. (very scuffed)
            var key = player.Value.PlayerSize + (0.0001f * float.Parse(player.Value.PlayerID.ToString()));
            _leaderboardList.Add(key, player.Value.PlayerName);
        }
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        foreach (var player in _leaderboardList.Reverse())
        {
            // Instantiate a leaderboard item.
            var leaderboardItem = Instantiate(LeaderboardItemPrefab, Content);
            var leaderboardItemController = leaderboardItem.GetComponent<LeaderboardItem>();
            
            // Set the player name and size.
            leaderboardItemController.SetPlayerName((_leaderboardList.IndexOfKey(player.Key) + 1).ToString() + ". " + player.Value);
            // leaderboardItemController.SetPlayerSize(player.Key);
        }
    }
}
