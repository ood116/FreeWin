using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkUserData : NetworkBehaviour
{
    // Nick Name
    [Networked] public string nickName { get; set; }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default) {  this.nickName = nickName; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) {
            RPC_SetNickName(UserData.instance.nickName);
        }
    }
}
