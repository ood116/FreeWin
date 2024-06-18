using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoSingleton<UserData>
{
    // NickName
    private string nickName;
    public string NickName
    {
        get 
        {
            if (nickName == null) {
                if (PlayerPrefs.HasKey("NickName")) NickName = PlayerPrefs.GetString("NickName");
                else NickName = "Test" + Random.Range(0, 999999);
            }
            return nickName; 
        }
        set 
        { 
            nickName = value;
            PlayerPrefs.SetString("NickName", value);
        }
    }

    [ContextMenu("PlayerPrefs Clear")]
    public void PlayerPrefsClear()
    {
        PlayerPrefs.DeleteAll();
    }
}
