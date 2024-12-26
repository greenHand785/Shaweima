using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject ESC_UI;
    public GameObject Main_UI;
    public TaskMgr m_TaskMgr;
    public ShipEquip m_Ship;
    public BotFactory m_Factory;
    public GoldSystem m_GoldSystem;
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

    bool ischanged;
    JsonSkillList skillList;
    private bool skillInit;
    private Ray _ray;
    private RaycastHit _raycastHit;

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
                if (isStart ==true)
                {
                    ECS_ACTIVE = false;
                }
            }
        }
    }

    void Start()
    {
        Debug.Log("游戏开始");
        isStartGame = false;
        list = ConfigManager.Instance.GetJsonConfig<JsonLevelList>(JsonConfigType.Json_LevelConfig);
        
        skillList = ConfigManager.Instance.GetJsonConfig<JsonSkillList>(JsonConfigType.Json_SkillConfig);
        skillItemPrefab = Resources.Load<GameObject>("Prefables/Skills/Skill");
    }

    // Update is called once per frame
    void Update()
    {
        //ESC();

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
        if (curTime >= info.totalTime)
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
        if(m_Ship.HP < 0)
        {
            // 结束游戏 返回主界面 TODO

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
        //m_Ship.ResetParam();
        m_TaskMgr.ResetParam();
        //m_Factory.ResetParam();
        //m_GoldSystem.ResetParam();
    }

    public void SetSkill()
    {
        if (currentSkillObj == null)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Destroy(currentSkillObj);
        //}

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
    public void QuiteGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }

    public bool ECS_ACTIVE;
    private void ESC()
    {
        
       if (Input.GetKeyDown(KeyCode.Escape))
       {
           ECS_ACTIVE = !ECS_ACTIVE;
           if (ECS_ACTIVE)
           {
               isStartGame = false;
           }
       }
       ESC_UI.gameObject.SetActive(ECS_ACTIVE);
       
        if (Main_UI.active==true)
        {
            ESC_UI.gameObject.SetActive(false);
        }
    }
}
