using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoSingleton<UIMgr>
{

    Canvas m_canvas;
    public Camera UICamera;
    // Start is called before the first frame update
    void Start()
    {
        m_canvas = transform.GetComponent<Canvas>();
    }

    

    public T InstanceUI<T>(string path) where T : MonoBehaviour
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(path));
        go.transform.SetParent(m_canvas.transform, false);
        return go.GetComponent<T>();
    }
}
