using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģ�鹦��
/// 1. ����ע������
/// 2. ��������
/// 3. ����ʱ�䷢������
/// </summary>
public class TaskMgr : MonoBehaviour
{
    public float m_createTastTime = 10;
    public float m_createAniTime = 3;
    public float m_taskHurtRange = 1;
    public float m_taskGoldRange = 1;


    private float m_curTime;

    public int randomCount = 3;

    private float m_aniCurTime;
    
    public int m_createCount = 3;
    public Transform m_createPos;
    public List<MonsterBot> m_aniList; // �����б�


    public List<EqupiBase> m_equips;

    private List<TaskBase> curTask; // ��ǰ�����б�
    public List<TaskBase> CurTask
    {
        get
        {
            int count = curTask.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if(curTask[i] == null)
                {
                    curTask.RemoveAt(i);
                }
            }
            return curTask;
        }
    }

    private Dictionary<ObjectBase, TaskBase> target;
    // Start is called before the first frame update
    void Start()
    {
        curTask = new List<TaskBase>();
        target = new Dictionary<ObjectBase, TaskBase>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltalTime = Time.deltaTime;
        m_curTime += deltalTime;
        if(m_curTime >= m_createTastTime)
        {
            m_curTime = 0;

            CreateTask();
        }

        m_aniCurTime += deltalTime;
        if (m_aniCurTime >= m_createAniTime)
        {
            m_aniCurTime = 0;
            CreateAni();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void CreateTask()
    {
        CreateMaintenanceTask();
        // TODO ������������

    }

    private void CreateAni()
    {
        if (m_aniList == null || m_aniList.Count <= 0)
        {
            return;
        }
        int index = Random.Range(0, m_aniList.Count);
        MonsterBot targetEqu = m_aniList[index];
        targetEqu = Instantiate(targetEqu.gameObject, m_createPos).GetComponent<MonsterBot>();
        targetEqu.transform.localPosition = Vector3.zero;

        int randomHp = Random.Range(0, 5);        
        for (int i = 0; i < randomCount; i++)
        {
            targetEqu.InitHP(randomHp);
        }
    }

    private void CreateMaintenanceTask()
    {
        if(m_equips == null || m_equips.Count <= 0)
        {
            return;
        }
        int index = Random.Range(0, m_equips.Count);
        EqupiBase targetEqu = m_equips[index];
        if (target.ContainsKey(targetEqu) && target[targetEqu] != null)
        {
            return;
        }
        MaintenanceTask task = CreateTaskExeample<MaintenanceTask>(targetEqu.gameObject);
        float hurt = Random.Range(m_taskHurtRange, m_taskHurtRange + 5);
        float gold = Random.Range(m_taskGoldRange, m_taskGoldRange + 2);
        float totalTime = Random.Range(60, 120);
        for (int i = 0; i < randomCount; i++)
        {
            int randomType = Random.Range(0, 3);
            float time = Random.Range(10, 15);
            task.AddRequire((ObjectType)randomType, time);
        }
        task.InitTask(targetEqu, hurt, gold, totalTime);

        curTask.Add(task);
        if (target.ContainsKey(targetEqu))
        {
            target[targetEqu] = task;
        }
        else
        {
            target.Add(targetEqu, task);
        }

        EventCenter.Broadcast(CombatEventType.Event_MaintenanceTaskCreate, task);
    }

    /// <summary>
    /// ��������ģ��
    /// </summary>
    private T CreateTaskExeample<T>(GameObject go)where T:TaskBase
    {
        T t = go.GetComponent<T>();
        if(t != null)
        {
            return t;
        }
        return go.AddComponent<T>();
    }


    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void ResetParam()
    {
        if(curTask != null)
        {
            foreach (var item in curTask)
            {
                Destroy(item);
            }
            curTask.Clear();
        }

        BotBase[] mons = FindObjectsOfType<BotBase>();
        foreach (var item in mons)
        {
            Destroy(item.gameObject);
        }
        m_curTime = 0;
        m_aniCurTime = 0;
    }
}
