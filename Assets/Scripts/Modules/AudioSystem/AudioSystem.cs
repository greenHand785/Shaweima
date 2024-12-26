using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioClip uiClick;//�����Ч
    public AudioClip create; // ������Ч
    public AudioClip upgrade; // ������Ч
    public AudioClip error; // ������Ч

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
    /// ����uiClick
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

    // ������Ʒ��Ч
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
