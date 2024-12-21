using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour

{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T), true) as T;
            }
            return instance;
        }
    }

}
