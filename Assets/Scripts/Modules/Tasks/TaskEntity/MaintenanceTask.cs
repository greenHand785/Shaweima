using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 维修任务基类
/// </summary>
public class MaintenanceTask : TaskBase
{
    protected Dictionary<ObjectType, TaskInfo> m_taskRequire;
    protected Dictionary<ObjectType, List<BotBase>> m_curworking;

    protected ObjectBase m_equip;
    protected float m_hurtValue;
    protected float m_cureValue;

    private TaskPanel m_task;

    public override bool CheckIsWorking()
    {
        if(m_taskRequire == null)
        {
            return true;
        }
        bool result = true;
        foreach (var item in m_taskRequire)
        {
            if (item.Value.isFinish)
            {
                continue;
            }
            if(m_curworking == null)
            {
                result = false;
                break;
            }
            if (!m_curworking.ContainsKey(item.Key))
            {
                result = false;
                break;
            }
            if(m_curworking[item.Key].Count <= 0)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public override void CheckLimitedTime()
    {
        // 检测
        float deltalTime = Time.deltaTime;
        float workTime = 0;
        if (CheckIsWorking())
        {
            deltalTime = 0;
        }
        m_curWaitTime += deltalTime;
        // 当所有类型的机器人都存在后，时间不在增加
        m_isFail = m_curWaitTime >= m_totalTime;

        bool isfinish = true;
        foreach (var item in m_taskRequire)
        {
            workTime = 0;
            if (m_curworking != null)
            {
                if (m_curworking.ContainsKey(item.Key))
                {
                    List<BotBase> list = m_curworking[item.Key];
                    float effectValue = 0;
                    foreach (var bot in list)
                    {
                        effectValue += bot.EfficiencyMultiplier;
                    }
                    workTime = Time.deltaTime * effectValue;
                }
            }
            item.Value.curTime += workTime;
            item.Value.isFinish = item.Value.curTime >= item.Value.totalTime;
            if (!item.Value.isFinish)
            {
                isfinish = false;
            }
        }
        m_isFinish = isfinish;
    }

    /// <summary>
    /// 获得任务剩余时间
    /// </summary>
    /// <returns></returns>
    public float GetReleaseTime()
    {
        return Mathf.Max(0, m_totalTime - m_curWaitTime);
    }

    public override void GetPunishment()
    {
        m_equip.Injured(m_hurtValue);
    }

    public override void GetRewards()
    {
        m_equip.Cure(m_cureValue);
    }

    /// <summary>
    /// 初始化任务
    /// </summary>
    public void InitTask(ObjectBase equip, float hurt, float cure, float totalTime)
    {
        m_hurtValue = hurt;
        m_cureValue = cure;
        m_equip = equip;
        m_totalTime = totalTime;

        m_task = UIMgr.Instance.InstanceUI<TaskPanel>("Prefables/Pop");
        m_task.SetTitle(equip.name);
        m_task.SetTask(this);
    }

    /// <summary>
    /// 添加需求,可以通过随机任务生成器，生成不同的需求以及时间
    /// </summary>
    public void AddRequire(ObjectType type, float time)
    {
        if(m_taskRequire == null)
        {
            m_taskRequire = new Dictionary<ObjectType, TaskInfo>();
        }
        if (!m_taskRequire.ContainsKey(type))
        {
            m_taskRequire.Add(type, new TaskInfo() { totalTime = time });
        }
        else
        {
            m_taskRequire[type].totalTime += time;
        }
    }

    /// <summary>
    /// 添加机器人
    /// </summary>
    public void AddBot(ObjectType type, BotBase bot)
    {
        if(bot == null)
        {
            Debug.LogError("bot null");
            return;
        }
        if (bot.isWorking)
        {
            Debug.LogError("机器人正在工作，无法重复添加");
            // todo 广播
            return;
        }
        if (m_taskRequire != null && !m_taskRequire.ContainsKey(type))
        {
            Debug.LogError("无需太机器人，无需添加");
            return;
        }
        if(m_curworking == null)
        {
            m_curworking = new Dictionary<ObjectType, List<BotBase>>();
        }
        if(!m_curworking.TryGetValue(type, out List<BotBase> list))
        {
            list = new List<BotBase>();
            m_curworking.Add(type, list);
        }
        list.Add(bot);
    }

    /// <summary>
    /// 移除机器人
    /// </summary>
    public void RemoveBot(ObjectType type, BotBase bot)
    {
        if (bot == null)
        {
            Debug.LogError("bot null");
            return;
        }
        if(m_curworking == null)
        {
            Debug.LogError("bot 不存在");
            return;
        }
        if(m_curworking.TryGetValue(type, out List<BotBase> list))
        {
            if (list.Contains(bot))
            {
                list.Remove(bot);
                // 移除成功
            }
        }
    }

    StringBuilder s = new StringBuilder();
    /// <summary>
    /// 初始化tag
    /// </summary>
    void InitTaskTag()
    {
        s.Length = 0;
        s.AppendLine("需求:");
        if(m_taskRequire != null)
        {
            foreach (var item in m_taskRequire)
            {
                s.AppendLine(item.Key.ToString() + ": " + item.Value.totalTime + "s");
            }
        }
        m_task.SetContent(s.ToString());
        // 设置目标
        m_task.SetTarget(m_equip.transform);
    }

    public bool IsContain(ObjectType type)
    {
        if(m_taskRequire == null)
        {
            return false;
        }
        return m_taskRequire.ContainsKey(type);
    }

    public Dictionary<ObjectType, TaskInfo> GetTaskInfos()
    {
        return m_taskRequire;
    }

    public override void Destroy()
    {
        Destroy(m_task.gameObject);
        base.Destroy();
    }

    protected override void Update()
    {
        base.Update();
        
        InitTaskTag();
    }


}
