using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    private string nickName;
    private string roomName;

    private string[] playerName = new string[10];

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

        // Get Join Players
        for(int i = 0; i < playerName.Length; ++i) {
            GUI.Button(new Rect (Screen.width - 200, 40 * i, 200, 40), playerName[i]);
        }
    }

    private void Awake()
    {
        instance = this;

        if (NetworkManager.instance.GetRunnerState() != NetworkRunner.States.Running) {
#if UNITY_EDITOR
            NetworkManager.instance.ConnectSession("Session_Editor", GameMode.AutoHostOrClient, callBack: GetSession);
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

    public void SetSessionPlayers()
    {
        if (!NetworkManager.instance.runner.IsServer) return;
        
        SessionInfo session = NetworkManager.instance.runner.SessionInfo;

        playerName[0] = "현재 " + NetworkManager.instance.NetworkPlayer.Count + "명 / 최대 " + session.MaxPlayers + "명";

        Debug.Log("123 " + NetworkManager.instance.NetworkPlayer.Count);
        int p_Num = 1;
        foreach (var playerKey in NetworkManager.instance.NetworkPlayer.Keys) {
            var value = NetworkManager.instance.NetworkPlayer[playerKey];
            Debug.Log("123 " + value.GetComponent<NetworkUserData>().nickName);
            playerName[p_Num++] = value.GetComponent<NetworkUserData>().nickName;
        }
        for (int i = p_Num; i < playerName.Length; ++i) {
            playerName[i] = "";
        }
    }

    private void OnEnable()
    {
        NetworkManager.instance.networkPlayerUpdateAction += SetSessionPlayers;
    }

    private void OnDisable()
    {
        NetworkManager.instance.networkPlayerUpdateAction -= SetSessionPlayers;
    }
}
