using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private void OnGUI()
    {
        // Go To Game
        if (GUI.Button(new Rect(0,100,200,40), "Host")) {
            Game();
        }

        // Go To Join
        if (GUI.Button(new Rect(0,150,200,40), "Join")) {
            Join();
        }
    }

    public void Game() => NetworkManager.instance.StartGame(Fusion.GameMode.Host);

    public void Join() => NetworkManager.instance.StartGame(Fusion.GameMode.Client);
}
