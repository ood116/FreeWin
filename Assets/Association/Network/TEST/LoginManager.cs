using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LoginManager : MonoBehaviour
{
    private string nickName;
    private UIControls uIControls;
    private UICreationControls uICreationControls;

    private void OnGUI()
    {
        // Set Nick Name
        GUI.Box(new Rect(0, 0, 200, 60), "NickName");
        nickName = GUI.TextField(new Rect (10, 20, 180, 30), nickName);
        
        // Go To Lobby
        if (GUI.Button(new Rect(0,70,200,40), "Go To Lobby")) {
            Login();
        }
    }

    private void Awake()
    {
        var CanvControl = GameObject.Find("CanvControl");
        uIControls = CanvControl.GetComponentInChildren<UIControls>();
        uICreationControls = CanvControl.GetComponentInChildren<UICreationControls>();
    }

    private void Start()
    {
        nickName = UserData.instance.nickName;
    }

    public void SetNickName() => UserData.instance.nickName = nickName;

    public void Login()
    {
        SetNickName();
        NetworkManager.instance.ConnectToLobby();
    }
}
