using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum BotState
{
    Free,
    Work,
    Walk,
    Dead
}

public class BotBase : ObjectBase
{
    public ObjectType type;
    public Mover mover;
    public  Animator animator; // Animator 组件引用
    protected MaintenanceTask m_targetTask;
    protected Transform m_curWorkPos;
    protected List<Vector3> tempWayPos = new List<Vector3>();


    public float MoveSpeed;//移动速度
    public float EfficiencyMultiplier;//工作效率，倍
    public float CanSurvivalTime;  //可存活时间
    public float Durability; //耐久度[0,1]， passTime/CanSurvivalTime；
    public int CreatNeedCoins;//花费金币
    public float passTime;     
   
    public BotState botState;
    /// <summary>
    /// 机器人工作状态
    /// </summary>
    public bool isWorking
    {
        get;
        private set;
    }

    protected void StartWork()
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
         if(botState==BotState.Dead) return;
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
        //当前在行走
        botState=BotState.Walk;
        transform.LookAt(m_curWorkPos);
    }



    public void CheckLife()
    {
        if(botState==BotState.Dead) return;
        passTime+=Time.deltaTime;
        if (passTime>=CanSurvivalTime)
        {
           botState=BotState.Dead;
            //销毁自己？
        }
    }

    public void AnimationCotrll()
    {
          animator.SetInteger("State", (int)botState);
    }
    public void CheckStartWork()
    {
        if(botState==BotState.Dead) return;
        if (m_targetTask==null) return;
        if (isWorking) return;
        if (Vector3.Distance(transform.position, m_curWorkPos.position)<0.1)
        {
            StartWork();
            mover.SetWork(false);
            //当前在工作
            botState=BotState.Work;
            transform.LookAt(m_targetTask.transform.position);
        }
    }
    protected void CheckLeaveWorkPonint()
    {

    }

    public void SetMoverSpeed(int speed)
    {
        MoveSpeed=speed;
    }

    public void SetProperty(JsonProductDefield info)
    {
        MoveSpeed = info.moveSpeed;
        EfficiencyMultiplier = info.effect;
        CanSurvivalTime = info.durability;
    }

    protected void Init()
    {
        mover=GetComponent<Mover>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }




    // Update is called once per frame
    void Update()
    {
        AnimationCotrll() ;
        CheckStartWork();
        CheckLeaveWorkPonint();
        CheckLife();

        mover.SetSpeed(MoveSpeed);
    }
}
