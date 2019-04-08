using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomLayoutGroup : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject roomListingPrefab { get { return _roomListingPrefab; } }

    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> roomListingButtons { get { return _roomListingButtons; } }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            RoomReceived(room);
        }
        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        int index = roomListingButtons.FindIndex(x => x.roomName == room.Name);
        Debug.Log(index);

        if(index == -1)
        {
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(roomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                roomListing.updated = true;
                roomListingButtons.Add(roomListing);

                index = (roomListingButtons.Count - 1);
            }
        }

        if(index != -1)
        {
            RoomListing roomListing = roomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach(RoomListing roomListing in roomListingButtons)
        {
            Debug.Log(roomListing.updated);
            if (!roomListing.updated)
            {
                Debug.Log("???");
                removeRooms.Add(roomListing);
            }
            else
            {
                roomListing.updated = false;
            }
        }

        foreach(RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            roomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }
}
