using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LoginManager : MonoBehaviour
{
    private string nickName;

    private void OnGUI()
    {
        // Set Nick Name
        nickName = GUI.TextField(new Rect (0, 0, 200, 40), nickName);
        
        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Login();
        }
    }

    private void Start()
    {
        nickName = UserData.Instance.nickName;
    }

    public void SetNickName() => UserData.Instance.nickName = nickName;

    public void Login()
    {
        SetNickName();
        NetworkManager.Instance.ConnectToLobby();
    }
}
