using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    private string nickName;
    private string roomName;

    private void OnGUI()
    {
        // Get Nick Name
        GUI.Box(new Rect(0, 0, 200, 60), "NickName");
        GUI.Button(new Rect (10, 20, 180, 30), nickName);

        // Get Room Name
        GUI.Box(new Rect(210, 0, 200, 60), "RoomName");
        GUI.Button(new Rect (220, 20, 180, 30), roomName);

        // Create Room
        if (GUI.Button(new Rect(210,70,200,40), "Go To Lobby")) {
            Lobby();
        }
    }

    private void Awake()
    {
        instance = this;

        if (NetworkManager.instance.GetRunnerState() != NetworkRunner.States.Running) {
#if UNITY_EDITOR
            NetworkManager.instance.ConnectSession("Session_Editor_" + Random.Range(0, 99999), GameMode.AutoHostOrClient, callBack: GetSession);
#else
            Lobby();
#endif
        }
        else if (NetworkManager.instance.GetRunnerState() == NetworkRunner.States.Running) {
            GetSession();
        }
    }

    public void Lobby() => NetworkManager.instance.DisConnectSession();

    public void GetSession()
    {
        nickName = UserData.instance.nickName;
        roomName = NetworkManager.instance.runner.SessionInfo.Name;
    }
}
