using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

enum ChatTag { Channel_All, Channel_1, Channel_2, Channel_3, Channel_4, Channel_5, Channel_6, Channel_7, Channel_8, Channel_9 };
public class ChatManager : NetworkBehaviour
{
    public GameObject chat_Prefab;
    public Transform chatContent;
    public TMP_InputField chatInput;
    [SerializeField] private ChatTag chatTag = ChatTag.Channel_All;

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
        RPC_MsgSend_Controll(msg, (int)chatTag);

        chatInput.text = "";
        chatInput.ActivateInputField();
    }

    public void RPC_MsgSend_Controll(string msg, int chatTag, Color? color = null)
    {
        Color msgColor = color ?? Color.black;
        RPC_MsgSend(msg, chatTag, msgColor);
    }
    
    // For All
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend(string msg, Color color)
    {
        InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    // For Tag (ChatTag)
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend(string msg, int chatTag, Color color)
    {
        if (this.chatTag == (ChatTag)chatTag) {
            InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
    }

    // For Tag (Player)
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend([RpcTarget] PlayerRef player, string msg, Color color)
    {
        InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    private GameObject InstantiateChat(string _msg)
    {
        GameObject chatObj = Instantiate(chat_Prefab, chatContent);
        chatObj.GetComponentInChildren<TextMeshProUGUI>().text = _msg;
        return chatObj;
    }
}