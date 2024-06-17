using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    private string nickName;

    private void OnGUI()
    {
        // Set Nick Name
        nickName = GUI.TextField(new Rect (0, 0, 200, 40), nickName);
        
        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Login(nickName);
        }
    }

    private void Awake()
    {
        nickName = "Test" + Random.Range(0, 999999);
    }

    public void Login(string nickName) => NetworkManager.instance.ConnectToLobby(nickName);
}
