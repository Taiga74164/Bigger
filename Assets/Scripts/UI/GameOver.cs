using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameOver : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _continueText;
    
    private void OnEnable()
    {
        if (!GameManager.Instance.IsGameOver)
            return;
        
        GameManager.Instance.IsGamePaused = true;
        _winnerText.SetText($"Winner: {GameManager.Instance.Winner.PlayerName}");
        StartCoroutine(ContinueCountdown());
    }
    
    public void Continue()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    
    private IEnumerator ContinueCountdown()
    {
        for (var i = 0; i < 5; i++)
        {
            _continueText.SetText($"Continue({5 - i}s)");
            yield return new WaitForSeconds(1);
        }
        
        Continue();
    }
}
