using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static object lockObject = new object();

    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null) {
                    instance = new GameObject().AddComponent<T>();
                    instance.name = typeof(T).ToString();
                    DontDestroyOnLoad(instance.gameObject);
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
    }
}