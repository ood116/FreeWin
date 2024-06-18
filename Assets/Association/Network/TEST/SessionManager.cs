using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SessionManager : MonoBehaviour
{
    private string roomName;

    private void OnGUI()
    {
        roomName = GUI.TextField(new Rect (0, 0, 200, 40), roomName);

        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Lobby();
        }
    }

    private void Awake()
    {
        if (NetworkManager.Instance.GetRunnerState() != NetworkRunner.States.Running) {
            roomName = "Test" + Random.Range(0, 999999);
            NetworkManager.Instance.ConnectToSession(UserData.Instance.NickName);
        }
        else if (NetworkManager.Instance.GetRunnerState() == NetworkRunner.States.Running) {
            // Get Current Session
        }
    }

    public void Lobby() => NetworkManager.Instance.DisConnectSession();
}
