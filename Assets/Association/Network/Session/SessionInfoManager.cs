using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class SessionInfoManager : NetworkBehaviour
{
    [Header("===Reference===")]
    [SerializeField] private UIControls uIControls;
    [SerializeField] private UICreationControls uICreationControls;
    private Vector2 ui_Position;
    private float ui_Width;
    private float ui_Height;

    [Header("===GotoLobby===")]
    [SerializeField] private GameObject button_Prefab;
    private Button gotoLobbyButton;

    [Header("===Info===")]
    [SerializeField] private GameObject sessionInfo_Prefab;
    [SerializeField] private GameObject playerInfo_Prefab;
    [SerializeField] private Transform playerInfoContent;
    private GameObject sessionInfo;
    private List<GameObject> playerInfo;

    private void Awake()
    {
        SetUISize();
        ContentSetting();
        SessionInfoSetting();
        PlayerInfoSetting();
        GotoLobbySetting();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => NetworkManager.instance.GetRunnerState() == NetworkRunner.States.Running);
        yield return new WaitUntil(() => NetworkManager.instance.runner.IsCloudReady);
        
        NetworkManager.instance.networkPlayerUpdateAction += SetSessionPlayers;
        NetworkManager.instance.networkPlayerUpdateAction?.Invoke();
    }

    private void SetUISize()
    {
        ui_Width = uIControls.sizeHor;
        ui_Height = uIControls.sizeVer;
        ui_Position = uIControls.selectArea.position;
    }

    private void ContentSetting()
    {
        // Init Content
        this.TryGetComponent<RectTransform>(out RectTransform rectTransform);
        rectTransform.position = new Vector2(ui_Position.x, ui_Position.y);
        rectTransform.sizeDelta = new Vector2(ui_Width, ui_Height * NetworkManager.instance.playerCount);
    }

    private void SessionInfoSetting()
    {
        // Init SessionInfo
        sessionInfo = Instantiate(sessionInfo_Prefab, this.transform);
        sessionInfo.TryGetComponent<RectTransform>(out RectTransform sessionInfoRect);
        sessionInfoRect.sizeDelta = new Vector2(ui_Width * 5f, ui_Height);
        sessionInfoRect.pivot = new Vector2(0, 1);
        sessionInfoRect.position = new Vector2(ui_Position.x + (ui_Width * 2f), ui_Position.y);
    }

    private void PlayerInfoSetting()
    {
        // Remove PlayerInfo
        foreach (Transform child in playerInfoContent) {
            Destroy(child.gameObject);
        }

        // Init PlayerInfo
        playerInfo = new List<GameObject>();
        
        for (int i = 0; i < NetworkManager.instance.playerCount; ++i) {
            GameObject p_Info = Instantiate(playerInfo_Prefab, playerInfoContent);
            p_Info.TryGetComponent<RectTransform>(out RectTransform p_InfoRect);
            p_InfoRect.sizeDelta = new Vector2(ui_Width, ui_Height);
            playerInfo.Add(p_Info);
        }
    }

    private void GotoLobbySetting()
    {
        Instantiate(button_Prefab, this.transform).TryGetComponent<Button>(out gotoLobbyButton);
        gotoLobbyButton.onClick.RemoveAllListeners();
        gotoLobbyButton.onClick.AddListener(() => NetworkManager.instance.DisConnectSession());
        gotoLobbyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Go To Lobby";

        gotoLobbyButton.TryGetComponent<RectTransform>(out RectTransform gotoLobbyButtonRect);
        gotoLobbyButtonRect.sizeDelta = new Vector2(ui_Width, ui_Height);
        gotoLobbyButtonRect.pivot = new Vector2(0, 1);
        gotoLobbyButtonRect.position = new Vector2(ui_Position.x + (ui_Width * 8f), ui_Position.y);
    }

    public void SetSessionPlayers()
    {
        SessionInfo session = NetworkManager.instance.runner.SessionInfo;
        TextMeshProUGUI sessionInfo_TMP = sessionInfo.GetComponentInChildren<TextMeshProUGUI>();
        sessionInfo_TMP.text = "[ " + session.Name + " ]" + " / 현재 " + NetworkManager.instance.NetworkPlayer.Count + "명 / 최대 " + session.MaxPlayers + "명";
        
        int p_Num = 0;
        string[] playerName = new string[NetworkManager.instance.playerCount];
        foreach (var playerKey in NetworkManager.instance.NetworkPlayer.Keys) {
            var value = NetworkManager.instance.NetworkPlayer[playerKey];
            var nick = value.GetComponent<NetworkUserData>().nickName;
            playerInfo[p_Num].GetComponentInChildren<TextMeshProUGUI>().text = nick;
            playerName[p_Num] = nick;
            p_Num++;
        }
        for (int i = p_Num; i < NetworkManager.instance.playerCount; ++i) {
            playerInfo[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            playerName[i] = "";
        }

        RPC_GetSessionPlayers(sessionInfo_TMP.text, playerName);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_GetSessionPlayers(string _sessionName, string[] _playerName)
    {
        // 나머지 플레이어 
        if (!NetworkManager.instance.runner.IsServer) {
            // Set SessionInfo
            this.sessionInfo.GetComponentInChildren<TextMeshProUGUI>().text = _sessionName;

            // Set PlayerInfo
            for (int i = 0; i < NetworkManager.instance.playerCount; ++i) {
                this.playerInfo[i].GetComponentInChildren<TextMeshProUGUI>().text = _playerName[i];
            }
        }
    }

    private void OnDisable()
    {
        if (!NetworkManager.instance.runner.IsServer) return;
        NetworkManager.instance.networkPlayerUpdateAction -= SetSessionPlayers;
    }
}
