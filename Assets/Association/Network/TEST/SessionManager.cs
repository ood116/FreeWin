using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SessionManager : MonoBehaviour
{
    private string roomName;

    private string[] playerName = new string[16];

    private void OnGUI()
    {
        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Lobby();
        }

        roomName = GUI.TextArea(new Rect (0, 0, 200, 40), roomName);

        for(int i = 0; i < playerName.Length; ++i) {
            playerName[i] = GUI.TextArea(new Rect (Screen.width - 200, 40 * i, 200, 40), playerName[i]);
        }
    }

    private void Awake()
    {
        if (NetworkManager.Instance.GetRunnerState() != NetworkRunner.States.Running) {
#if UNITY_EDITOR
            NetworkManager.Instance.ConnectSession("Session_Editor", GameMode.AutoHostOrClient, callBack: GetSessionName);
#else
            Lobby();
#endif
        }
        else if (NetworkManager.Instance.GetRunnerState() == NetworkRunner.States.Running) {
            GetSessionName();
        }
    }

    public void Lobby() => NetworkManager.Instance.DisConnectSession();

    public void GetSessionName()
    {
        roomName = NetworkManager.Instance.runner.SessionInfo.Name;
    }

    public void GetSessionPlayers()
    {
        SessionInfo session = NetworkManager.Instance.runner.SessionInfo;

        for(int i = 0; i < session.PlayerCount; ++i) {
            
        }
    }
}
