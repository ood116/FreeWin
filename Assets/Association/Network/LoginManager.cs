using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    private string nickName = "Test";

    private void OnGUI()
    {
        // Set Nick Name
        nickName = GUI.TextField(new Rect (0, 0, 200, 50), nickName);
        
        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Login(nickName);
        }

        // Go To Game
        if (GUI.Button(new Rect(0,100,200,40), "Host")) {
            Game();
        }

        // Go To Join
        if (GUI.Button(new Rect(0,150,200,40), "Join")) {
            Join();
        }
    }

    private void Awake()
    {
        nickName = "Test" + Random.Range(0, 999999);
    }

    public void Login(string nickName) => NetworkManager.instance.ConnectToLobby(nickName);

    public void Game() => NetworkManager.instance.StartGame(Fusion.GameMode.Host);

    public void Join() => NetworkManager.instance.StartGame(Fusion.GameMode.Client);
}
