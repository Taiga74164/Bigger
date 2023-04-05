using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Singleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T _instance;
    
    /// <summary>
    /// Returns the left hand side of the ??= operator if it is not null, otherwise it returns the right hand side.
    /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
    /// </summary>
    public static T Instance => _instance ??= FindObjectOfType<T>();
    
    protected virtual void OnDestroy() => _instance = null;
}