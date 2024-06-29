using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Unity.Burst.Intrinsics;

public class SessionConnectManager : MonoBehaviour
{
    [Header("===Reference===")]
    [SerializeField] private UIControls uIControls;
    [SerializeField] private UICreationControls uICreationControls;
    private Vector2 ui_Position;
    private float ui_Width;
    private float ui_Height;

    [Header("===Common===")]
    [SerializeField] private GameObject button_Prefab;
    [SerializeField] private GameObject inputField_Prefab;
    private TMP_InputField sessionInputField;

    [Header("===SessionCreate===")]
    private Button sessionCreateButton;

    [Header("===SessionJoin===")]
    private Button sessionJoinButton;

    private void Awake()
    {
        SetUISize();
        CommonSetting();
        SessionConnectSetting();
        SessionJoinSetting();
    }

    private void SetUISize()
    {
        ui_Width = uIControls.sizeHor;
        ui_Height = uIControls.sizeVer;
        ui_Position = uIControls.selectArea.position;
    }

    private void CommonSetting()
    {
        Instantiate(inputField_Prefab, this.transform).TryGetComponent<TMP_InputField>(out sessionInputField);
        sessionInputField.GetComponentInChildren<TMP_InputField>().text = "Session_" + Random.Range(0, 999999);

        sessionInputField.TryGetComponent<RectTransform>(out RectTransform sessionInputFieldRect);
        sessionInputFieldRect.sizeDelta = new Vector2(ui_Width * 3f, ui_Height);
        sessionInputFieldRect.pivot = new Vector2(0, 1);
        sessionInputFieldRect.position = new Vector2(ui_Position.x + (ui_Width * 1f), ui_Position.y);
    }

    private void SessionConnectSetting()
    {
        Instantiate(button_Prefab, this.transform).TryGetComponent<Button>(out sessionCreateButton);
        sessionCreateButton.onClick.RemoveAllListeners();
        sessionCreateButton.onClick.AddListener(() => NetworkManager.instance.ConnectSession(sessionInputField.text, GameMode.Host));
        sessionCreateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Create Session";

        sessionCreateButton.TryGetComponent<RectTransform>(out RectTransform sessionCreateButtonRect);
        sessionCreateButtonRect.sizeDelta = new Vector2(ui_Width, ui_Height);
        sessionCreateButtonRect.pivot = new Vector2(0, 1);
        sessionCreateButtonRect.position = new Vector2(ui_Position.x + (ui_Width * 5f), ui_Position.y);
    }

    private void SessionJoinSetting()
    {
        Instantiate(button_Prefab, this.transform).TryGetComponent<Button>(out sessionJoinButton);
        sessionJoinButton.onClick.RemoveAllListeners();
        sessionJoinButton.onClick.AddListener(() => NetworkManager.instance.ConnectSession(sessionInputField.text, GameMode.AutoHostOrClient));
        sessionJoinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Session Join";

        sessionJoinButton.TryGetComponent<RectTransform>(out RectTransform sessionJoinButtonRect);
        sessionJoinButtonRect.sizeDelta = new Vector2(ui_Width, ui_Height);
        sessionJoinButtonRect.pivot = new Vector2(0, 1);
        sessionJoinButtonRect.position = new Vector2(ui_Position.x + (ui_Width * 6f), ui_Position.y);
    }
}   
