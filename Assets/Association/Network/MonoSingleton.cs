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

                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null) {
                    _instance = new GameObject().AddComponent<T>();
                    _instance.name = typeof(T).ToString();
                }
                return _instance;
            }
        }
    }

    private void Awake()
    {
        if (instance == this) {
            DontDestroyOnLoad(instance.gameObject);
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