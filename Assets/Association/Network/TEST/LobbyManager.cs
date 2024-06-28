using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LobbyManager : MonoBehaviour
{
    private string nickName;
    private string roomName;

    private void OnGUI()
    {
        // Get Nick Name
        GUI.Box(new Rect(0, 0, 200, 60), "NickName");
        GUI.Button(new Rect (10, 20, 180, 30), nickName);

        // Set Room Name
        GUI.Box(new Rect(210, 0, 200, 60), "RoomName");
        roomName = GUI.TextField(new Rect (220, 20, 180, 30), roomName);

        // Create Room
        if (GUI.Button(new Rect(210,70,200,40), "Create Room")) {
            Create();
        }

        // Join Room
        if (GUI.Button(new Rect(210,120,200,40), "Join Room")) {
            Join();
        }
    }

    private void Awake()
    {
        nickName = UserData.instance.nickName;
        roomName = "Session_" + Random.Range(0, 999999);
    }

    public void Create() => NetworkManager.instance.ConnectSession(roomName, GameMode.Host);

    public void Join() => NetworkManager.instance.ConnectSession(roomName, GameMode.AutoHostOrClient);
}
