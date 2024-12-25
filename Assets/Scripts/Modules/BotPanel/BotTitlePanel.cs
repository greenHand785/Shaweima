using Component.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotTitlePanel : MonoBehaviour
{
    public Text level;
    public Text name;
    public Slider hpSlider;
    public Slider workingSlider;

    public UIFollowObject follow;

    private BotBase bot;
    private MaintenanceTask tasks;
    private TaskInfo taskInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bot == null)
        {
            Destroy(gameObject);
            return;
        }
        level.text = bot.Level.ToString();
        name.text = bot.name.ToString();
        float curHp = bot.CanSurvivalTime - bot.passTime;
        curHp = Mathf.Max(0, curHp);
        hpSlider.value = curHp;
        if(taskInfo != null)
        {
            curHp = taskInfo.totalTime - taskInfo.curTime;
            curHp = Mathf.Max(0, curHp);
            workingSlider.value = curHp;
            workingSlider.gameObject.SetActive(true);
        }
        else
        {
            workingSlider.gameObject.SetActive(false);
        }
    }

    public void InitTitlePanel(BotBase bot)
    {
        this.bot = bot;
        follow.target = bot.uiPos.transform;
        follow.showCamera = UIMgr.Instance.UICamera;
        hpSlider.maxValue = bot.CanSurvivalTime;
        
    }

    public void SetTask(MaintenanceTask task)
    {
        if(task == null)
        {
            return;
        }
        this.tasks = task;
        taskInfo = task.GetTaskInfo(bot.type);
        workingSlider.maxValue = taskInfo.totalTime;
    }
}
