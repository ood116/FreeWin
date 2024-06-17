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
    private List<SessionInfo> sessionInfo = new List<SessionInfo>();

    private void Awake()
    {
        previous_Page_Button.onClick.AddListener(() => Previous_Page());
        next_Page_Button.onClick.AddListener(() => Next_Page());
    }

    private void Start()
    {
        UpdateRoomList();
    }

    private void Previous_Page()
    {
        --currentPage;
        UpdateRoomList();
    }

    private void Next_Page()
    {
        ++currentPage;
        UpdateRoomList();
    }

    private void JoinRoom(string sessionName) 
    {
        NetworkManager.instance.ConnectToSession(sessionName);
    }

    private void UpdateRoomList()
    {
        // Calculate Page
        maxPage = (sessionInfo.Count % roomButton.Length == 0) ? sessionInfo.Count / roomButton.Length : sessionInfo.Count / roomButton.Length + 1;

        // Button Setting
        previous_Page_Button.interactable = (currentPage <= 1) ? false : true;
        next_Page_Button.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * roomButton.Length;
        for (int i = 0; i < roomButton.Length; ++i) {
            if (multiple + i < sessionInfo.Count) {
                roomButton[i].GetComponentInChildren<TextMeshProUGUI>().text = sessionInfo[multiple + i].Name;
                roomButton[i].interactable = true;

                roomButton[i].onClick.RemoveAllListeners();
                roomButton[i].onClick.AddListener(() => JoinRoom(sessionInfo[multiple + i].Name));
            }
            else {
                roomButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                roomButton[i].interactable = false;
            }
        }
    }

#region  Get SessionList from NetworkManager
    private void SessionListUpdate(List<SessionInfo> sessionList)
    {
        sessionInfo = sessionList.ToList();
        UpdateRoomList();
    }

    private void OnEnable()
    {
        NetworkManager.instance.sessionListUpdateAction += SessionListUpdate;
    }

    private void OnDisable()
    {
        NetworkManager.instance.sessionListUpdateAction -= SessionListUpdate;
    }
#endregion
}
