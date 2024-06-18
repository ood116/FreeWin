using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoSingleton<NetworkManager>, INetworkRunnerCallbacks
{
    private NetworkRunner runner;
    public Action<List<SessionInfo>> sessionListUpdateAction;

    private void Awake()
    {
        Screen.SetResolution(800, 600, false);
        runner = this.AddComponent<NetworkRunner>();
        GetRunnerState();
    }
    
    public NetworkRunner.States GetRunnerState() { return runner.State; }

#region Login -> Lobby
    async public void ConnectToLobby(string nickName)
    {
        // Go to Lobby
        var result = await runner.JoinSessionLobby(SessionLobby.Shared, "Defualt");

        if (result.Ok) {
            SceneManager.LoadScene("Assets/Association/_Scene/Lobby.unity");
        }
    }
#endregion

#region Lobby -> Session (Host)
    async public void CreateSession(string sessionName)
    {
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = sessionName,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) {
            SceneManager.LoadScene("Assets/Association/_Scene/Session.unity");
        }
    }
#endregion

#region Lobby -> Session (Auto)
    async public void ConnectToSession(string sessionName)
    {
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = sessionName,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) {
            SceneManager.LoadScene("Assets/Association/_Scene/Session.unity");
        }
    }
#endregion

#region Session -> Lobby
    async public void DisConnectSession()
    {
        await runner.Shutdown(true, ShutdownReason.Ok);
        ConnectToLobby(UserData.Instance.NickName);
    }

#endregion

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //runner.ProvideInput = true;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        sessionListUpdateAction?.Invoke(sessionList);
    }

#region Not Using
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
#endregion
}
