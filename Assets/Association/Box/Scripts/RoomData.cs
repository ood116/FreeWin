using UnityEngine;

public class RoomData : MonoBehaviour
{
    /*
    public Text counseleeText;
    public Text counselorText;
    public Text playerCountText;
    public Button joinButton;
    private RoomInfo roomInfo;
    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
        set
        {
            roomInfo = value;
            // data[0] = Counselee Name, data[1] = Counselee Birth, 
            // data[2] = Counselor Name, data[3] = Counselor Birth
            string[] data = value.Name.Split('_');
            counseleeText.text = data[0];
            counselorText.text = data[2];
            playerCountText.text = value.PlayerCount + " / " + value.MaxPlayers;
            if (value.PlayerCount == value.MaxPlayers) {
                joinButton.interactable = false;
            }
            else {
                joinButton.interactable = true;
            }
        }
    }

    private void Awake()
    {
        joinButton.onClick.AddListener(() => OnEnterRoom(roomInfo.Name));
    }

    public void OnEnterRoom(string roomName, RoomOptions roomOptions = null)
    {
        serverSingleton.SetLoading(true, "방 입장 중");
        //PhotonNetwork.JoinRoom(roomName, null);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }
    */
}
