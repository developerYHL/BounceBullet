using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CurrentRoomCanvas : MonoBehaviourPunCallbacks {

    public void OnClickStartSync()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void OnClickStartDelayed()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }
}
