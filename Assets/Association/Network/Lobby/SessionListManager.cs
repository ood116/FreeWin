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

    [Header("===Session Button===")]
    [Tooltip("방 버튼 부모")] public Transform sessionlistPannel;
    [Tooltip("방 버튼 부모")] public GameObject session_Prefab;
    [Tooltip("방 버튼 개수 설정")] public int sessionButtonCount = 8;
    private List<Button> sessionButton;

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
        // Init Size
        sessionlistPannel.TryGetComponent<GridLayoutGroup>(out GridLayoutGroup gridLayoutGroup);
        gridLayoutGroup.cellSize = new Vector2(uIControls.sizeHor * 3f, uIControls.sizeVer);

        sessionlistPannel.TryGetComponent<RectTransform>(out RectTransform sessionlistPannelRect);
        sessionlistPannelRect.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x * 2, gridLayoutGroup.cellSize.y * Mathf.CeilToInt(sessionButtonCount / 2));

        // Init Position
        this.TryGetComponent<RectTransform>(out RectTransform rectTransform);
        rectTransform.position = new Vector2(uIControls.selectArea.position.x + uIControls.sizeHor * 3f, uIControls.selectArea.position.y + uIControls.sizeVer * -10f);

        // Init Pre, Next Button
        previous_Page_Button.TryGetComponent<RectTransform>(out RectTransform preRectTransform);
        preRectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
        preRectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - sessionlistPannelRect.sizeDelta.y);

        next_Page_Button.TryGetComponent<RectTransform>(out RectTransform nextRectTransform);
        nextRectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y);
        nextRectTransform.position = new Vector2(rectTransform.position.x + preRectTransform.rect.width, rectTransform.position.y - sessionlistPannelRect.sizeDelta.y);
        
        // Remove Room Button
        foreach (Transform child in sessionlistPannel) {
            Destroy(child.gameObject) ;
        }

        // Init Room Button
        sessionButton = new List<Button>();
        for (int i = 0; i < sessionButtonCount; ++i) {
            GameObject roomBtn = Instantiate(session_Prefab, sessionlistPannel);
            sessionButton.Add(roomBtn.GetComponent<Button>());
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
        maxPage = (sessionList.Count % sessionButton.Count == 0) ? sessionList.Count / sessionButton.Count : sessionList.Count / sessionButton.Count + 1;

        // Button Setting
        previous_Page_Button.interactable = (currentPage <= 1) ? false : true;
        next_Page_Button.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * sessionButton.Count;
        for (int i = 0; i < sessionButton.Count; ++i) {
            int num = i;
            if (multiple + i < sessionList.Count) {
                sessionButton[i].GetComponentInChildren<TextMeshProUGUI>().text = sessionList[multiple + num].Name;
                sessionButton[i].interactable = true;

                sessionButton[i].onClick.RemoveAllListeners();
                sessionButton[i].onClick.AddListener(() => JoinRoom(sessionList[multiple + num].Name));
            }
            else {
                sessionButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                sessionButton[i].interactable = false;
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
        SessionListUpdate(NetworkManager.instance.SessionList);
    }

    private void OnDisable()
    {
        NetworkManager.instance.sessionListUpdateAction -= SessionListUpdate;
    }
#endregion
}
