using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    public void SetPlayerName(string playerName) => this.GetComponent<TMP_Text>()!.text = playerName;
}
