using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        roomName = "Test" + Random.Range(0, 999999);
    }

    public void Create() => NetworkManager.instance.CreateSession(roomName);

    public void Join() => NetworkManager.instance.ConnectToSession(roomName);
}
