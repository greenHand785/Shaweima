using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public TaskMgr m_TaskMgr;
    public ShipEquip m_Ship;
    public BotFactory m_Factory;
    public GoldSystem m_GoldSystem;

    public int curLevel;

    private float curTime;
    JsonLevelList list;
    JsonLevelInfo info;

    bool ischanged;

    private bool isStart = true;
    public bool isStartGame
    {
        get
        {
            return isStart;
        }
        set
        {
            if(value != isStart)
            {
                isStart = value;
                EventCenter.Broadcast(CombatEventType.Event_GameStateChanged, isStart);
            }
        }
    }

    void Start()
    {
        Debug.Log("游戏开始");
        list = ConfigManager.Instance.GetJsonConfig<JsonLevelList>(JsonConfigType.Json_LevelConfig);
        isStartGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(list == null || list.levels == null)
        {
            return;
        }
        if (curLevel < 0 || curLevel >= list.levels.Count)
        {
            return;
        }
        Time.timeScale = 0;
        if (!isStartGame)
        {
            return;
        }
        Time.timeScale = 1;
        info = ConfigManager.Instance.GetJsonSheetConfig<JsonLevelInfo>(JsonSheetType.Json_LevelSheet, list.levels[curLevel]);
        info.level = curLevel;
        m_TaskMgr.m_createTastTime = info.createTaskTime;
        m_TaskMgr.m_createAniTime = info.createAniTime;
        m_TaskMgr.m_taskHurtRange = info.hurtCount;
        m_TaskMgr.m_taskGoldRange = info.taskGoldRange;

        curTime += Time.deltaTime;
        if (curTime >= info.totalTime|| m_Ship.HP <= 0)
        {
            curLevel++;
            curTime = 0;
            // 结束
            isStartGame = false;
            // 初始化
            ResetGameObject();
            EventCenter.Broadcast(CombatEventType.Event_LevelOver);
            // test
            EventCenter.Broadcast(CombatEventType.Event_GameStart);
            isStartGame = true;
        }
    }

    public float GetReleaseTime()
    {
        if(info == null)
        {
            return 0;
        }
        return Mathf.Max(0, info.totalTime - curTime);
    }

    public void ResetGameObject()
    {
        m_Ship.ResetParam();
        m_TaskMgr.ResetParam();
        m_Factory.ResetParam();
        m_GoldSystem.ResetParam();
    }

}
