using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoSingleton<UIMgr>
{

    Canvas m_canvas;
    public Camera UICamera;
    public Button factoryBtn;
    public Transform popRoot;

    public BotFactoryPanel FactoryUI;
    // Start is called before the first frame update
    void Start()
    {
        m_canvas = transform.GetComponent<Canvas>();
        factoryBtn.onClick.AddListener(OnClickFactoryBtn);
    }

    

    public T InstanceUI<T>(string path) where T : MonoBehaviour
    {
        GameObject go = Instantiate(Resources.Load<GameObject>(path));
        go.transform.SetParent(popRoot, false);
        return go.GetComponent<T>();
    }

    void OnClickFactoryBtn()
    {
        if (FactoryUI.state)
        {
            FactoryUI.Close();
        }
        else
        {
            FactoryUI.Open();
        }
    }
}
