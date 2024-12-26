using Component.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    private Text title;
    private Text releaseTime; // 剩余时间

    private UIFollowObject follow;
    private string title_str;
    private string content_str;
    private MaintenanceTask task;

    public Transform subTaskRoot;
    public SuTaskPanelNew exampleSubTask;
    
    // Start is called before the first frame update
    void Start()
    {
        title = transform.Find("title").GetComponent<Text>();
        releaseTime = transform.Find("title/releaseCount").GetComponent<Text>();
        subTaskRoot = transform.Find("SubTask").transform;
        follow = transform.GetComponent<UIFollowObject>();
    }

    private void Update()
    {
        title.text = title_str;
        releaseTime.text = task.GetReleaseTime().ToString("F1");
        if(task == null)
        {
            Destroy(gameObject);
        }
    }
    public void SetTitle(string str)
    {
        title_str = str;
    }

    public void SetContent(string str)
    {
        content_str = str;
    }

    public void SetTask(MaintenanceTask task)
    {
        this.task = task;
        exampleSubTask.gameObject.SetActive(false);
        // 初始化子任务
        if (task.GetTaskInfos() != null)
        {
            foreach (var item in task.GetTaskInfos())
            {
                SuTaskPanelNew sub = Instantiate(exampleSubTask.gameObject, subTaskRoot).GetComponent<SuTaskPanelNew>();
                sub.SetTaskInfo(item.Key, item.Value, task);
                sub.gameObject.SetActive(true);

                //SuTaskPanelNew sub = UIMgr.Instance.InstanceUI<SuTaskPanelNew>("Prefables/Pop1");
                //sub.SetTaskInfo(item.Key, item.Value, task);
                //sub.SetTarget(task.transform);
            }
        }

    }

    public void SetTarget(Transform target)
    {
        follow.target = target;
        follow.showCamera = UIMgr.Instance.UICamera;
    }
}
