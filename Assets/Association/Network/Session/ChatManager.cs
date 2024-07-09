using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

enum ChatTag { Channel_All, Channel_1, Channel_2, Channel_3, Channel_4, Channel_5, Channel_6, Channel_7, Channel_8, Channel_9 };
public class ChatManager : NetworkBehaviour
{
    // Chat UI
    public RectTransform rect;
    public RectTransform scrollView;
    public RectTransform chatContent;
    public TMP_InputField chatInput;
    [SerializeField] private ChatTag chatTag = ChatTag.Channel_All;
    
    // Chat Pool
    public GameObject chat_Prefab;
    private Queue<GameObject> chatQueue = new Queue<GameObject>();
    private int chatPoolSize = 200;

    // Auto Scroll
    private float autoScrollOffset = 100f;

    /***********************************************/

    private void Awake()
    {
        this.TryGetComponent<RectTransform>(out rect);
    }

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
            float previous_Content_Height = chatContent.sizeDelta.y;
            float previous_Content_anchoredPositionY = chatContent.anchoredPosition.y;
            InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
            StartCoroutine(AutoScrollDown(previous_Content_Height, previous_Content_anchoredPositionY));
        }
    }

    // For Tag (Player)
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MsgSend([RpcTarget] PlayerRef player, string msg, Color color)
    {
        InstantiateChat(msg).GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    // Auto ScrollDown
    private IEnumerator AutoScrollDown(float previous_Content_Height, float previous_Content_anchoredPositionY)
    {
        yield return null;

        if (chatContent.sizeDelta.y > scrollView.sizeDelta.y + rect.sizeDelta.y) {
            if (previous_Content_Height - previous_Content_anchoredPositionY < rect.sizeDelta.y + autoScrollOffset) {
                chatContent.anchoredPosition = new Vector2(0, chatContent.sizeDelta.y - scrollView.sizeDelta.y);
            }
            else {
                // 이전 대화 보고 있을 시 자동스크롤X
                // 새 메세지 알림
            }
        }
    }

    private GameObject InstantiateChat(string _msg)
    {
        GameObject chatObj = null;
        if (chatQueue.Count < chatPoolSize) {
            chatObj = Instantiate(chat_Prefab, chatContent);
            chatObj.GetComponentInChildren<TextMeshProUGUI>().text = _msg;
            chatQueue.Enqueue(chatObj);
        }
        else {
            chatObj = chatQueue.Dequeue();
            chatObj.GetComponentInChildren<TextMeshProUGUI>().text = _msg;
            chatObj.transform.SetAsLastSibling();
            chatQueue.Enqueue(chatObj);
        }
        return chatObj;
    }
}