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
    //指示技能位置预制体
    public GameObject skillItemPrefab;
    //指示技能位置预制体
    public GameObject skillItem;
    //未释放的技能（已生成但是未释放）
    public GameObject currentSkillObj;

    public int curLevel;

    private float curTime;
    JsonLevelList list;
    JsonLevelInfo info;

    JsonSkillList skillList;
    private bool skillInit;
    private Ray _ray;
    private RaycastHit _raycastHit;

    public bool isStartGame;

    void Start()
    {
        Debug.Log("游戏开始");
        list = ConfigManager.Instance.GetJsonConfig<JsonLevelList>(JsonConfigType.Json_LevelConfig);

        skillList = ConfigManager.Instance.GetJsonConfig<JsonSkillList>(JsonConfigType.Json_SkillConfig);
        skillItemPrefab = Resources.Load<GameObject>("Prefables/Skills/Skill");
    }

    // Update is called once per frame
    void Update()
    {
        SetSkill();

        if (skillPanelControl.skills.Length > 0 && !skillInit)
        {
            for (int i = 0; i < skillList.skillList.Count; i++)
            {
                skillPanelControl.skills[i].info = ConfigManager.Instance.GetJsonSheetConfig<JsonSkillInfo>(JsonSheetType.Json_SkillSheet, skillList.skillList[i]);
                skillPanelControl.skills[i].GetPrefabInfo();
            }

            skillInit = true;
        }

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

        if (Physics.Raycast(_ray, out _raycastHit, 1000f, ~(1 << 8)))
        {
            currentSkillObj.transform.position = _raycastHit.point;
            skillItem.transform.position = _raycastHit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentSkillObj.GetComponent<SkillItem>().UseSkill();
            currentSkillObj = null;

            Destroy(skillItem);
            skillItem = null;
        }
    }
}
