using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioClip uiClick;//点击音效
    public AudioClip create; // 创建音效
    public AudioClip upgrade; // 升级音效
    public AudioClip error; // 错误音效

    public AudioSource UISource;

    public AudioSource bgSource;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(CombatEventType.Event_OnClickUI, OnClickUI);
        EventCenter.AddListener(CombatEventType.Event_OnCreate, CreateProduct);
        EventCenter.AddListener(CombatEventType.Event_OnUpgrade, Upgrade);
        EventCenter.AddListener(CombatEventType.Event_OnError, Error);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(CombatEventType.Event_OnClickUI, OnClickUI);
        EventCenter.RemoveListener(CombatEventType.Event_OnCreate, CreateProduct);
        EventCenter.RemoveListener(CombatEventType.Event_OnUpgrade, Upgrade);
        EventCenter.AddListener(CombatEventType.Event_OnError, Error);
    }

    /// <summary>
    /// 播放uiClick
    /// </summary>
    public void PlayUIClickSource()
    {
        UISource.clip = uiClick;
        UISource.Play();
    }

    void OnClickUI()
    {
        UISource.clip = uiClick;
        UISource.Play();
    }

    // 创建产品音效
    void CreateProduct()
    {
        UISource.clip = create;
        UISource.Play();
    }

    void Upgrade()
    {
        UISource.clip = upgrade;
        UISource.Play();
    }
    void Error()
    {
        UISource.clip = error;
        UISource.Play();
    }
}
