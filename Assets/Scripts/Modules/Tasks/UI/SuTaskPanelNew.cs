using Component.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuTaskPanelNew : MonoBehaviour
{
    public Image bgIcon;
    public Image botIcon;
    public Image sliderIcon;
    public Slider slider;

    private TaskInfo info;
    private ObjectType type;

    public UIFollowObject follow;

    private MaintenanceTask task;
    // Update is called once per frame
    void Update()
    {
        slider.maxValue = info.totalTime;
        float curV = info.totalTime - info.curTime;
        curV = Mathf.Max(0, curV);
        slider.value = curV;

        BotFactoryInfoBase product = BotFactory.Instance.GetFactoryInfoByType(type);
        if (botIcon.sprite == null)
        {
            botIcon.sprite = Resources.Load<Sprite>(product.info.iconPath);
        }
        sliderIcon.color = product.info.workingProcessCol;
        bgIcon.color = product.info.headCol;
        //if(task == null)
        //{
        //    Destroy(gameObject);
        //}
    }

    public void SetTaskInfo(ObjectType objType, TaskInfo taskInfo, MaintenanceTask task)
    {
        info = taskInfo;
        type = objType;
        this.task = task;
    }

    public void SetTarget(Transform target)
    {
        follow.target = target;
        follow.showCamera = UIMgr.Instance.UICamera;
    }
}
