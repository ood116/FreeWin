using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoSingleton<UserData>
{
    // NickName
    private string _nickName;
    public string nickName
    {
        get 
        {
            if (_nickName == null) {
                if (PlayerPrefs.HasKey("NickName"))
                    nickName = PlayerPrefs.GetString("NickName");
                else 
                    nickName = "Guest" + Random.Range(0, 999999);
            }
            return _nickName; 
        }
        set 
        { 
            _nickName = value;
            PlayerPrefs.SetString("NickName", value);
        }
    }
}
