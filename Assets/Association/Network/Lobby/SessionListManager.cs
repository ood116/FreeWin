using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListManager : MonoBehaviour
{
    public Button[] roomButton;
    public Button previous_Page_Button, next_Page_Button;
    int currentPage = 1, maxPage, multiple;
    public List<SessionInfo> sessionInfo;

    private void Awake()
    {
        previous_Page_Button.onClick.AddListener(() => Previous_Page());
        next_Page_Button.onClick.AddListener(() => Next_Page());
        RoomButtonSetting();
        sessionInfo = NetworkManager.instance.sessionInfo;
    }

    private void RoomButtonSetting()
    {
        for (int i = 0; i < roomButton.Length; ++i) {
            int num = i;
            roomButton[i].gameObject.AddComponent<SessionData>();
            roomButton[i].onClick.AddListener(() => RoomButtonClick(num));
        }
    }

    public void UpdateRoomList()
    {
        if (sessionInfo == null) return;

        // 최대페이지
        maxPage = (sessionInfo.Count % roomButton.Length == 0) ? sessionInfo.Count / roomButton.Length : sessionInfo.Count / roomButton.Length + 1;

        // 이전, 다음버튼
        previous_Page_Button.interactable = (currentPage <= 1) ? false : true;
        next_Page_Button.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * roomButton.Length;
        for (int i = 0; i < roomButton.Length; i++)
        {
            roomButton[i].interactable = (multiple + i < sessionInfo.Count) ? true : false;
            roomButton[i].GetComponentInChildren<TextMeshProUGUI>().text = (multiple + i < sessionInfo.Count) ? sessionInfo[multiple + i].Name : "";
        }
    }
    
    public void Previous_Page()
    {
        --currentPage;
        UpdateRoomList();
    }

    public void Next_Page()
    {
        ++currentPage;
        UpdateRoomList();
    }
    public void RoomButtonClick(int roomButtonIndex) 
    {
        Debug.Log(sessionInfo[multiple + roomButtonIndex]);
        UpdateRoomList();
    }

    private void OnEnable()
    {
        NetworkManager.instance.sessionListManager = this;
    }

    private void OnDisable()
    {
        NetworkManager.instance.sessionListManager = null;
    }
}
