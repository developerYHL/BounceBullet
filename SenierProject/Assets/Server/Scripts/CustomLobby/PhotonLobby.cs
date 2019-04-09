using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks {

    public static PhotonLobby instance;

    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();

        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    void RemoveRoomListings()
    {
        while(roomsPanel.childCount != 0)
        {
            Destroy(roomsPanel.GetChild(0).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen && room.IsVisible)
        {
            Debug.Log("creat");
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.setRoom();
        }
    }

    public void CreateRoom()
    {
        Debug.Log("룸 생성");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패");
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }

    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }

    public void JoinLobbyOnClick()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.Log("join");
        }
    }
}
