using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
