using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject PlayerPrefab;

    public float MinX;
    public float MaxX;
    
    public float MinY;
    public float MaxY;

    private void Start()
    {
        // SpawnPosition
        var position = new Vector3();
        PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position, Quaternion.identity);
    }

}
