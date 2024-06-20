using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LobbyManager : MonoBehaviour
{
    private string roomName;

    private void OnGUI()
    {
        roomName = GUI.TextField(new Rect (0, 0, 200, 40), roomName);

        // Create Room
        if (GUI.Button(new Rect(0,50,200,40), "Create")) {
            Create();
        }

        // Join Room
        if (GUI.Button(new Rect(0,100,200,40), "Join")) {
            Join();
        }
    }

    private void Awake()
    {
        roomName = "Session_" + Random.Range(0, 999999);

        if (NetworkManager.Instance.GetRunnerState() != NetworkRunner.States.Starting) {
            NetworkManager.Instance.ConnectToLobby();
        }
    }

    public void Create() => NetworkManager.Instance.ConnectSession(roomName, GameMode.Host);

    public void Join() => NetworkManager.Instance.ConnectSession(roomName, GameMode.AutoHostOrClient);
}
