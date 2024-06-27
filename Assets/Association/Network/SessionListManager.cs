using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListManager : MonoBehaviour
{
    [Header("===Reference===")]
    [SerializeField] private UIControls uIControls;
    [SerializeField] private UICreationControls uICreationControls;

    [Header("===Room Button===")]
    [Tooltip("방 버튼 부모")] public Transform roomlistPannel;
    [Tooltip("방 버튼 부모")] public GameObject room_Prefab;
    [Tooltip("방 버튼 개수 설정")] public int roomButtonCount = 8;
    private List<Button> roomButton;

    [Header("===Pgae Button===")]
    [SerializeField] private Button previous_Page_Button;
    [SerializeField] private Button next_Page_Button;
    int currentPage = 1, maxPage, multiple;
    private List<SessionInfo> sessionList = new List<SessionInfo>();

    private void Awake()
    {
        RoomButtonSetting();
        
        previous_Page_Button.onClick.AddListener(() => Previous_Page());
        next_Page_Button.onClick.AddListener(() => Next_Page());
    }

    private void RoomButtonSetting()
    {
        // Set Size
        roomlistPannel.TryGetComponent<GridLayoutGroup>(out GridLayoutGroup gridLayoutGroup);
        gridLayoutGroup.cellSize = new Vector2(uIControls.sizeHor * 3f, uIControls.sizeVer);

        roomlistPannel.TryGetComponent<RectTransform>(out RectTransform roomlistPannelRect);
        roomlistPannelRect.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x * 2, gridLayoutGroup.cellSize.y * Mathf.CeilToInt(roomButtonCount / 2));

        // Set Position
        this.TryGetComponent<RectTransform>(out RectTransform rectTransform);
        rectTransform.position = new Vector2(uIControls.selectArea.position.x + uIControls.sizeHor * 3f, uIControls.selectArea.position.y + uIControls.sizeVer * -10f);

        previous_Page_Button.TryGetComponent<RectTransform>(out RectTransform preRectTransform);
        preRectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
        preRectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - roomlistPannelRect.sizeDelta.y);

        next_Page_Button.TryGetComponent<RectTransform>(out RectTransform nextRectTransform);
        nextRectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
        nextRectTransform.position = new Vector2(rectTransform.position.x + preRectTransform.rect.width, rectTransform.position.y - roomlistPannelRect.sizeDelta.y);
        
        // Remove Room Button
        foreach (Transform child in roomlistPannel) {
            Destroy(child.gameObject) ;
        }

        // Set Room Button
        roomButton = new List<Button>();
        for (int i = 0; i < roomButtonCount; ++i) {
            GameObject roomBtn = Instantiate(room_Prefab, roomlistPannel);
            roomButton.Add(roomBtn.GetComponent<Button>());
        }
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
        NetworkManager.instance.ConnectSession(sessionName, GameMode.AutoHostOrClient);
    }

    private void UpdateRoomList()
    {
        // Calculate Page
        maxPage = (sessionList.Count % roomButton.Count == 0) ? sessionList.Count / roomButton.Count : sessionList.Count / roomButton.Count + 1;

        // Button Setting
        previous_Page_Button.interactable = (currentPage <= 1) ? false : true;
        next_Page_Button.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * roomButton.Count;
        for (int i = 0; i < roomButton.Count; ++i) {
            int num = i;
            if (multiple + i < sessionList.Count) {
                roomButton[i].GetComponentInChildren<TextMeshProUGUI>().text = sessionList[multiple + num].Name;
                roomButton[i].interactable = true;

                roomButton[i].onClick.RemoveAllListeners();
                roomButton[i].onClick.AddListener(() => JoinRoom(sessionList[multiple + num].Name));
            }
            else {
                roomButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                roomButton[i].interactable = false;
            }
        }
    }

#region Get SessionList from NetworkManager
    private void SessionListUpdate(List<SessionInfo> sessionList)
    {
        this.sessionList.Clear();
        this.sessionList = sessionList.ToList();
        UpdateRoomList();
    }

    private void OnEnable()
    {
        NetworkManager.instance.sessionListUpdateAction += SessionListUpdate;
        SessionListUpdate(NetworkManager.instance.sessionList);
    }

    private void OnDisable()
    {
        NetworkManager.instance.sessionListUpdateAction -= SessionListUpdate;
    }
#endregion
}
