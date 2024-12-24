using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public Text Year;
    public Text Gold;
    public TaskMgr m_TaskMgr;

    public int curLevel;

    private float curTime;
    JsonLevelList list;
    JsonLevelInfo info;

    public bool isStartGame;

    void Start()
    {
        Debug.Log("游戏开始");
        list = ConfigManager.Instance.GetJsonConfig<JsonLevelList>(JsonConfigType.Json_LevelConfig);

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
        if (!isStartGame)
        {
            return;
        }
        info = ConfigManager.Instance.GetJsonSheetConfig<JsonLevelInfo>(JsonSheetType.Json_LevelSheet, list.levels[curLevel]);
        m_TaskMgr.m_createTastTime = info.createTaskTime;


        curTime += Time.deltaTime;
        if (curTime >= info.totalTime)
        {
            curLevel++;
            curTime = 0;
            // 结束
            isStartGame = false;
            EventCenter.Broadcast(CombatEventType.Event_LevelOver);
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






}
