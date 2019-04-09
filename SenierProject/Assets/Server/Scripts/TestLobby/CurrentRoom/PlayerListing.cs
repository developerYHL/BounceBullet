using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks {

    public Player PhotonPlayer { get; private set; }

    [SerializeField]
    private Text _playerName;
    private Text playerName { get { return _playerName; } }

    public void ApplyPhotonPlayer(Player photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        playerName.text = photonPlayer.NickName;
    }
}
