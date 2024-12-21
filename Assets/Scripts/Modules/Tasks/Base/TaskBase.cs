using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBase : MonoBehaviour, ITask
{
    protected float m_curWaitTime;
    protected float m_curWorkTime;
    protected float m_totalTime = 60; // ��ʱ��
    protected bool m_isFinish; // �Ƿ��������
    protected bool m_isFail; // �Ƿ�ʧ��
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
        // ���������͵Ļ����˶����ں�ʱ�䲻������
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
    /// ��������
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
