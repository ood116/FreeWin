using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class ChatManager : NetworkBehaviour, IPlayerJoined
{
    public GameObject chat_Prefab;
    public Transform chatContent;
    public TMP_InputField chatInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            // 엔터 치면 isFcused -> flase되어서 역으로 처리
            if (!chatInput.isFocused) MsgSend();
            // 채팅기능이 열여 있으면 엔터로 포커싱
            if (chatInput.IsInteractable()) chatInput.ActivateInputField();
        }  
    }

    public void MsgSend()
    {
        if (chatInput.text.Length == 0) return;

        string msg = "[" + UserData.instance.nickName + "]" + " : " + chatInput.text;
        RPC_MsgSend(msg);

        chatInput.text = "";
        chatInput.ActivateInputField();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend(string msg)
    {
        InstantiateChat(msg);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend(string msg, Color color)
    {
        InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    private GameObject InstantiateChat(string msg)
    {
        GameObject chatObj = Instantiate(chat_Prefab, chatContent);
        chatObj.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        return chatObj;
    }

    public void PlayerJoined(PlayerRef player)
    {
        throw new System.NotImplementedException();
    }
}
