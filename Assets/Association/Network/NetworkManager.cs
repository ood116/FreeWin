using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class NetworkManager : MonoSingleton<NetworkManager>, INetworkRunnerCallbacks
{
    public NetworkRunner runner;

    // Session
    [ReadOnly] public int playerCount = 16;
    public Action<List<SessionInfo>> sessionListUpdateAction;
    public List<SessionInfo> sessionList = new List<SessionInfo>();

    // Player
    public Action networkPlayerUpdateAction;
    public Dictionary<PlayerRef, NetworkObject> networkPlayer = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, NetworkObject> NetworkPlayer
    {
        get { return networkPlayer; }
        set 
        {   
            networkPlayer = value;
            networkPlayerUpdateAction?.Invoke(); 
        }
    }
    
    // ETC
    private bool isConnecting = false;

    private void Awake()
    {
        Screen.SetResolution(800, 600, false);
        runner = this.AddComponent<NetworkRunner>();
        GetRunnerState();
    }
    
    public NetworkRunner.States GetRunnerState() { return runner.State; }

#region Login -> Lobby
    async public void ConnectToLobby(Action callBack = null)
    {
        if (isConnecting) return;

        // Go to Lobby
        var result = await runner.JoinSessionLobby(SessionLobby.Shared, "Defualt");
        
        // Call Back
        if (result.Ok) {
            SceneManager.LoadScene("Assets/Association/_Scene/Lobby.unity");
            runner.ProvideInput = false;
            callBack?.Invoke();
            isConnecting = false;
        }
    }
#endregion

#region Lobby -> Session
    async public void ConnectSession(string sessionName, GameMode gameMode, string sceneName = "Session", Action callBack = null)
    {
        if (isConnecting) return;

        // Set Session Scene
        string sessionSceneName = "Assets/Association/_Scene/" + sceneName + ".unity";
        var scene = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(sessionSceneName));
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start Game Session
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            CustomLobbyName = "Defualt",
            PlayerCount = playerCount,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        // Call Back
        if (result.Ok) {
            runner.ProvideInput = true;
            callBack?.Invoke();
            isConnecting = false;
        }
    }
#endregion

#region Session -> Lobby
    async public void DisConnectSession()
    {
        if (isConnecting) return;

        // Reset And Goto Lobby
        await runner.Shutdown(true, ShutdownReason.Ok);
        ConnectToLobby();
    }
#endregion

#region Event
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer) {
            GameObject playerObj = Resources.Load<GameObject>("Prefabs/Player");
            NetworkObject networkPlayerObject = runner.Spawn(playerObj, Vector2.zero, Quaternion.identity, player);
            networkPlayerObject.name = networkPlayerObject.InputAuthority.ToString();
            NetworkPlayer.Add(player, networkPlayerObject);
            //networkPlayerUpdateAction?.Invoke(); // 닉네임 이 바로 변경 되지 않기 때문에 개선 필요 (현재 Player - NetworkUserData 에서 작동 됨)
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (NetworkPlayer.TryGetValue(player, out NetworkObject networkObject)) {
            runner.Despawn(networkObject);
            NetworkPlayer.Remove(player);
            networkPlayerUpdateAction?.Invoke();
        }
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        this.sessionList.Clear();
        this.sessionList = sessionList.ToList();
        sessionListUpdateAction?.Invoke(sessionList);
    }
#endregion

    public NetworkObject GetLocalPlayer()
    {
        return runner.GetPlayerObject(runner.LocalPlayer);
    }

#region Not Using Event
    // Debug.Log(MethodBase.GetCurrentMethod().Name);
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

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
#endregion
}
