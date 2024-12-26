using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum BotState
{
    Free,
    Work,
    Walk,
    Dead,
    Attack,
    BeDamege

}

public class BotBase : ObjectBase
{
    public ObjectType type;
    public Mover mover;
    public  Animator animator; // Animator �������
    protected MaintenanceTask m_targetTask;
    protected Transform m_curWorkPos;
    protected List<Vector3> tempWayPos = new List<Vector3>();

    protected BotTitlePanel ui;

    public float Level;
    public float MoveSpeed;//�ƶ��ٶ�
    public float EfficiencyMultiplier;//����Ч�ʣ���
    public float CanSurvivalTime;  //�ɴ��ʱ��
    public float Durability; //�;ö�[0,1]�� passTime/CanSurvivalTime��
    public int CreatNeedCoins;//���ѽ��
    public float passTime;

    public GameObject uiPos;
    public GameObject WorkVFX;
   
 
    public BotState botState;//{protected set;get;}
    /// <summary>
    /// �����˹���״̬
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
    /// ���ù���״̬
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
        //��ǰ������
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
            //�����Լ���
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
            //��ǰ�ڹ���
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
        Level = info.level;
    }

    protected virtual void Init() 
    {
        mover=GetComponent<Mover>();
        ui = UIMgr.Instance.InstanceUI<BotTitlePanel>("Prefables/BotTitlePanel");
        ui.InitTitlePanel(this);
    }


    // Start is called before the first frame update
   public void Start()
    {
        Init();
    }

  


    // Update is called once per frame
   public void Update()
    {
        AnimationCotrll() ;
        CheckStartWork();
        CheckLeaveWorkPonint();
        CheckLife();

        mover.SetSpeed(MoveSpeed);
        if(WorkVFX!=null)
        {
            WorkVFX.gameObject.SetActive(botState==BotState.Work);
        }
        if (botState==BotState.Dead)
        {
            print("机器人"+transform.name+"死亡");
            Destroy(gameObject,2);
        }

        // 设置任务
        ui.SetTask(m_targetTask);
    }
}
