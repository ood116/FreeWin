using System.Collections;
using UnityEngine;

public class CreatRoom : MonoBehaviour
{
    RoomManager RM;

    public void CreateRoom()
    {
        /*
        if (roomNameInputField.text.Trim() == "") {
                serverSingleton.SetPopup("방 이름이 비어 있습니다.", "방 이름을 입력 후 다시 시도해");
                return;
        }
        createRoom.SetActive(false);
        serverSingleton.SetLoading(true, "방 개설중");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = (int)maxPlayerSlider.value;
        roomOptions.CustomRoomProperties = new Hashtable() {
            {"Subject", dateOfbirthInputField.text}, 
            {"Description", descriptionInputField.text} };
        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text.Trim()
                                    + '_' + dateOfbirthInputField.text.Trim()
                                    + '_' + AccountData.Instance.userName, roomOptions, null);
        */
    }
}
