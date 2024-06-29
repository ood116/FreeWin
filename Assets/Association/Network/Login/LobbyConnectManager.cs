using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyConnectManager : MonoBehaviour
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
    [SerializeField] private GameObject inputField_Prefab;
    private TMP_InputField nickNameInputField;
    
    private void Awake()
    {
        SetUISize();
        NickNameInputFieldSetting();
        LobbyJoinSetting();
    }

    private void SetUISize()
    {
        ui_Width = uIControls.sizeHor;
        ui_Height = uIControls.sizeVer;
        ui_Position = uIControls.selectArea.position;
    }

    private void NickNameInputFieldSetting()
    {
        Instantiate(inputField_Prefab, this.transform).TryGetComponent<TMP_InputField>(out nickNameInputField);
        nickNameInputField.GetComponentInChildren<TMP_InputField>().text = UserData.instance.nickName;

        nickNameInputField.TryGetComponent<RectTransform>(out RectTransform nickNameInputFieldRect);
        nickNameInputFieldRect.sizeDelta = new Vector2(ui_Width * 3f, ui_Height);
        nickNameInputFieldRect.pivot = new Vector2(0, 1);
        nickNameInputFieldRect.position = new Vector2(ui_Position.x + (ui_Width * 1f), ui_Position.y);
    }

    private void LobbyJoinSetting()
    {
        Instantiate(button_Prefab, this.transform).TryGetComponent<Button>(out gotoLobbyButton);
        gotoLobbyButton.onClick.RemoveAllListeners();
        gotoLobbyButton.onClick.AddListener(() => Login());
        gotoLobbyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Login";

        gotoLobbyButton.TryGetComponent<RectTransform>(out RectTransform gotoLobbyButtonRect);
        gotoLobbyButtonRect.sizeDelta = new Vector2(ui_Width, ui_Height);
        gotoLobbyButtonRect.pivot = new Vector2(0, 1);
        gotoLobbyButtonRect.position = new Vector2(ui_Position.x + (ui_Width * 5f), ui_Position.y);
    }

    public void Login()
    {
        UserData.instance.nickName = nickNameInputField.text;
        NetworkManager.instance.ConnectToLobby();
    }
}
