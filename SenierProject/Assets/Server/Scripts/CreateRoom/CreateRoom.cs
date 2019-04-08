using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks {

    [SerializeField]
    private Text _roomName;
    private Text roomName { get { return _roomName; } }

    public void OnClickCreatedRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 8 };
        
        if (PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default))
        {
            print("룸 생성 성공 sent");
        }
        else
        {
            print("룸 생성 실패 send");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print(message);
    }

    public override void OnCreatedRoom()
    {
        print("룸 생성 성공");
    }
}
