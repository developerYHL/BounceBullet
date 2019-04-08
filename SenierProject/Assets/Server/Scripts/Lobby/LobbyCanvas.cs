using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyCanvas : MonoBehaviourPunCallbacks {

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup roomLayoutGroup { get { return _roomLayoutGroup; } }

    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName)) {

        }
        else
        {
            print("룸 입장 실패");
        }
    }
}
