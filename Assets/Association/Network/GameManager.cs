using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnGUI()
    {
        // Go To Lobby
        if (GUI.Button(new Rect(0,50,200,40), "Lobby")) {
            Lobby();
        }
    }

    public void Lobby() => NetworkManager.instance.DisConnectSession();
}
