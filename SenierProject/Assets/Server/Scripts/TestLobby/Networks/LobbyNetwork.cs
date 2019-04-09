using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviourPunCallbacks {

    private void Start()
    {
        print("서버 연결중...");
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 연결중...");
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.NickName = PlayerNetwork.instance.playerName;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비 연결됨");
        if (!PhotonNetwork.InRoom)
            MainCanvasManager.instance.LobbyCanvas.transform.SetAsLastSibling();
    }
}
