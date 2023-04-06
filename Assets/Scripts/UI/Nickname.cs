using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nickname : MonoBehaviour
{
    private void Awake()
    {
        var nickname = PlayerPrefsManager.Nickname;
        if (string.IsNullOrEmpty(nickname))
            return;
        
        this.GetComponent<TMP_InputField>().text = nickname;
    }
    
    public void SetNickname(string nickname)
    {
        PlayerPrefsManager.Nickname = nickname;
        PlayerPrefsManager.Save();
    }
}
