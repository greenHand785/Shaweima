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



    public int MoveSpeed;//移动速度
    public float EfficiencyMultiplier;//工作效率，倍
    public float CanSurvivalTime;  //可存活时间
    public float Durability; //耐久度[0,1]， passTime/CanSurvivalTime；
    public int CreatNeedCoins;//花费金币
    private float passTime;     
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
            m_targetTask.RemoveBot(type, this);
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



    private void CheckWorkState()
    {
        if (m_targetTask==null)
        {

        }
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

    public void SetMoverSpeed(int speed)
    {
        MoveSpeed=speed;
    }
    private void Init()
    {
        mover=GetComponent<Mover>();
        mover.SetSpeed(MoveSpeed);
    }
        // Start is called before the first frame update
    void Start()
    {
        Init();
    }




    // Update is called once per frame
    void Update()
    {
        CheckStartWork();
        CheckLeaveWorkPonint();
        CheckWorkState();
    }
}
