using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private Text _roomNameText;
    private Text roomNameText { get { return _roomNameText; } }
    public string roomName { get; private set; }
    public bool updated { get; set; }

	void Start () {
        GameObject lobbyCanvasObj = MainCanvasManager.instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObj == null)
            return;

        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(roomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        roomName = text;
        roomNameText.text = roomName;
    }
}
