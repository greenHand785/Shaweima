using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBase : ObjectBase
{
    public ObjectType type;
    public Mover mover;

    private MaintenanceTask m_targetTask;
    private Transform m_curWorkPos;
    private List<Vector3> tempWayPos = new List<Vector3>();

    /// <summary>
    /// 机器人工作状态
    /// </summary>
    public bool isWorking
    {
        get;
        private set;
    }

    private void StartWork()
    {
        m_targetTask.AddBot(type, this);
        isWorking = true;
    }
    /// <summary>
    /// 设置工作状态
    /// </summary>
    public void SetWorkState(bool workState)
    {
        isWorking = workState;
    }

    public void SetTargetTask(MaintenanceTask task, Transform targetWorkPoint)
    {
       
        if(m_curWorkPos!=null)
        {
             if (m_curWorkPos == targetWorkPoint) return;
            m_curWorkPos.GetComponentInParent<WorkArea>().ReleaseWorkPoint(m_curWorkPos);
        }
        m_targetTask = task;
        m_curWorkPos = targetWorkPoint;
        tempWayPos.Clear();
        tempWayPos.Add(transform.position);
        tempWayPos.Add(targetWorkPoint.position);
        mover.SetPathPoints(tempWayPos);
        isWorking = false;
        mover.SetWork(true);
    }
    public void CheckStartWork()
    {
        if (m_targetTask==null) return;
        if (isWorking) return;
        if (Vector3.Distance(transform.position, m_curWorkPos.position)<0.1)
        {
            StartWork();
            mover.SetWork(false);
        }
    }
    private void CheckLeaveWorkPonint()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckStartWork();
        CheckLeaveWorkPonint();
    }
}
