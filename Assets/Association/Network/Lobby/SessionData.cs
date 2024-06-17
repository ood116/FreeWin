using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionData : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    private SessionInfo sessionInfo;
    public SessionInfo SessionInfo
    {
        get
        {
            return sessionInfo;
        }
        set
        {
            sessionInfo = value;
            roomName.text = value.Name;
        }
    }

    public void JoinRoom()
    {
        /*
        NetworkManager.instance.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName.text,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        */
    }
}
