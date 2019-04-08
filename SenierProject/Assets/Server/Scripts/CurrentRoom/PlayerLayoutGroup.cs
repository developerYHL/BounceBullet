using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerLayoutGroup : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject playerListingPrefab { get { return _playerListingPrefab; } }

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> playerListings { get { return _playerListings; } }

    public override void OnMasterClientSwitched(Player photonPlayer)
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        MainCanvasManager.instance.currentRoomCanvas.transform.SetAsLastSibling();
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        for(int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    public override void OnLeftRoom()
    {
        foreach( PlayerListing playerLinting in playerListings)
        {
            Destroy(playerLinting.gameObject);
        }
        playerListings.Clear();
    }

    public override void OnPlayerEnteredRoom(Player photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    public override void OnPlayerLeftRoom(Player photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(Player photonPlayer)
    {
        if (photonPlayer == null)
            return;

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(playerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        playerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(Player photonPlayer)
    {
        Debug.Log("요깅네");
        int index = playerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index != -1)
        {
            Destroy(playerListings[index].gameObject);
            playerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = !PhotonNetwork.CurrentRoom.IsOpen;
        PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.IsOpen;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
