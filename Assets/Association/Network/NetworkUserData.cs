using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkUserData : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(NicknameChanged))]
    public string networkedNickname { get; set; } = UserData.Instance.nickName;
    void NicknameChanged() { UserData.Instance.nickName = networkedNickname; }

    void Start()
    {
        networkedNickname = UserData.Instance.nickName;
    }
}
