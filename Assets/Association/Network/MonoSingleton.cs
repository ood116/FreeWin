using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static object lockObject = new object();
    private static bool isQuitting = false;

    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (isQuitting) return null;

                if (instance == null) {
                    GameObject singletonObj = new GameObject();
                    instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString();
                    DontDestroyOnLoad(singletonObj.gameObject);
                }
                return instance;
            }
        }
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void OnApplicationQuit()
    {
        instance = null;
        isQuitting = true;
    }
}