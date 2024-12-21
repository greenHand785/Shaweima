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
    private float m_curTime;

    public int randomCount = 3;

    public List<ObjectBase> m_equips;

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
        m_curTime += Time.deltaTime;
        if(m_curTime >= m_createTastTime)
        {
            m_curTime = 0;

            CreateTask();
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

    private void CreateMaintenanceTask()
    {
        if(m_equips == null || m_equips.Count <= 0)
        {
            return;
        }
        int index = Random.Range(0, m_equips.Count);
        ObjectBase targetEqu = m_equips[index];
        if (target.ContainsKey(targetEqu) && target[targetEqu] != null)
        {
            return;
        }

        MaintenanceTask task = CreateTaskExeample<MaintenanceTask>(targetEqu.gameObject);
        int hurt = Random.Range(0, 5);
        int cure = Random.Range(0, 5);
        float totalTime = Random.Range(60, 120);
        task.InitTask(targetEqu, hurt, cure, totalTime);
        for (int i = 0; i < randomCount; i++)
        {
            int randomType = Random.Range(0, 3);
            float time = Random.Range(10, 15);
            task.AddRequire((ObjectType)randomType, time);
        }

        curTask.Add(task);
        if (target.ContainsKey(targetEqu))
        {
            target[targetEqu] = task;
        }
        else
        {
            target.Add(targetEqu, task);
        }
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
}
