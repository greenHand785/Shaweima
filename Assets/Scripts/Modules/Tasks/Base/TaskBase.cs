using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBase : MonoBehaviour, ITask
{
    protected float m_curWaitTime;
    protected float m_curWorkTime;
    protected float m_totalTime = 60; // 总时间
    protected bool m_isFinish; // 是否完成任务
    protected bool m_isFail; // 是否失败
    public virtual void CheckLimitedTime()
    {
        float deltalTime = Time.deltaTime;
        float workTime = 0;
        if (CheckIsWorking())
        {
            deltalTime = 0;
            workTime = Time.deltaTime;
        }
        m_curWaitTime += deltalTime;
        m_curWorkTime += workTime;
        // 当所有类型的机器人都存在后，时间不在增加
        m_isFail = m_curWorkTime >= m_totalTime;
        m_isFinish = m_curWorkTime >= m_totalTime;
    }

    public virtual bool CheckIsWorking()
    {
        return false;
    }

    public virtual void GetPunishment()
    {

    }

    public virtual void GetRewards()
    {

    }

    /// <summary>
    /// 销毁任务
    /// </summary>
    public virtual void Destroy()
    {
        Destroy(this);
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        CheckLimitedTime();
        if (m_isFinish)
        {
            GetRewards();
            Destroy();
        }
        if (m_isFail)
        {
            GetPunishment();
            Destroy();
        }
    }
}
