using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    [Header("===Reference===")]
    [SerializeField] private UIControls uIControls;
    [SerializeField] private UICreationControls uICreationControls;

    private void Awake()
    {
        instance = this;

        if (NetworkManager.instance.GetRunnerState() != NetworkRunner.States.Running) {
#if UNITY_EDITOR
            NetworkManager.instance.ConnectSession("Session_Editor_" + Random.Range(0, 99999), GameMode.AutoHostOrClient);
#else
            Lobby();
#endif
        }
    }

    public void Lobby() => NetworkManager.instance.DisConnectSession();
}
