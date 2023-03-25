using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Google.Protobuf;
using Photon.Realtime;

public class PlayerDataHandler : MonoBehaviour, IPunObservable
{
    private PhotonView _photonView;
    private PlayerController _playerController;
    private PlayerData _playerData;
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _playerController = GetComponent<PlayerController>();
        
        // Set up player data.
        _playerData = new PlayerData()
        {
            PlayerID = _photonView.Owner.ActorNumber,
            PlayerName = _photonView.Owner.NickName,
            PlayerSize = _playerController.Size
        };
    }
    
    private void Update()
    {
        if (!_photonView.IsMine)
            return;
        
        // Update player data.
        _playerData.PlayerSize = _playerController.Size;
        
        // Serialize player data.
        var bytes = _playerData.ToByteArray();
        
        // Send player data to other players.
        _photonView.RPC(nameof(OnPlayerDataUpdated), RpcTarget.Others, bytes);
    }
    
    [PunRPC]
    private void OnPlayerDataUpdated(byte[] bytes)
    {
        // Deserialize player data.
        var playerData = PlayerData.Parser.ParseFrom(bytes);
        
        // Update the player size attribute.
        _playerController.Size = playerData.PlayerSize;
    }
    
    /// <summary>
    /// Called when the PhotonView associated with this MonoBehaviour is about to serialize its view.
    /// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_photon_transform_view.html
    /// </summary>
    /// <param name="stream">The object used to read and write data.</param>
    /// <param name="info">The message that is being sent.</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write data to the stream.
            var bytes = _playerData.ToByteArray();
            stream.SendNext(bytes);
        }
        else
        {
            // Read data from the stream.
            var bytes = (byte[]) stream.ReceiveNext();
            var playerData = PlayerData.Parser.ParseFrom(bytes);
            
            // Update the player size attribute.
            _playerController.Size = playerData.PlayerSize;
        }
    }
}