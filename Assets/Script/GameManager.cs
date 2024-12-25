using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public Text Year;
    public Text Gold;
    public TaskMgr m_TaskMgr;
    public SkillPanelControl skillPanelControl;
    //δ�ͷŵļ��ܣ������ɵ���δ�ͷţ�
    public GameObject currentSkillObj;

    public int curLevel;

    private float curTime;
    JsonLevelList list;
    JsonLevelInfo info;

    JsonSkillList skillList;
    private Ray _ray;
    private RaycastHit _raycastHit;

    public bool isStartGame;

    void Start()
    {
        Debug.Log("��Ϸ��ʼ");
        list = ConfigManager.Instance.GetJsonConfig<JsonLevelList>(JsonConfigType.Json_LevelConfig);

        skillList = ConfigManager.Instance.GetJsonConfig<JsonSkillList>(JsonConfigType.Json_SkillConfig);
        for (int i = 0; i < skillList.skillList.Count; i++)
        {
            skillPanelControl.skills[i].info = ConfigManager.Instance.GetJsonSheetConfig<JsonSkillInfo>(JsonSheetType.Json_SkillSheet, skillList.skillList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetSkill();

        if (list == null || list.levels == null)
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
            // ����
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


    public void SetSkill()
    {
        if (currentSkillObj == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentSkillObj);
        }

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _raycastHit, 1000f))
        {
            if (_raycastHit.transform.tag == "Ground" || _raycastHit.transform.tag == "Enemy")
                currentSkillObj.transform.position = _raycastHit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentSkillObj = null;
        }
    }



}
