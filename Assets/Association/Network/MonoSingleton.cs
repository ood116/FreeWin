using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected MonoSingleton() { }
    private static T _instance = null;
    private static object lockObject = new object();
    private static bool isQuitting = false;

    public static T instance
    {
        get
        {
            lock (lockObject)
            {
                if (isQuitting) return null;

                if (_instance == null) {
                    GameObject singletonObj = new GameObject();
                    _instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString();
                    DontDestroyOnLoad(singletonObj.gameObject);
                }
                return _instance;
            }
        }
    }

    private void OnDisable()
    {
        _instance = null;
    }

    private void OnApplicationQuit()
    {
        _instance = null;
        isQuitting = true;
    }
}