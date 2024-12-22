using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubTaskPanel : MonoBehaviour
{
    private TaskInfo info;

    private Text nameTxt;
    private Slider curProgress;
    private Text curValueTxe;

    private ObjectType type;
    // Start is called before the first frame update
    void Start()
    {
        nameTxt = transform.GetComponent<Text>();
        curProgress = transform.Find("Slider").GetComponent<Slider>();
        curValueTxe = transform.Find("Value").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        curProgress.maxValue = info.totalTime;
        float curV = info.totalTime - info.curTime;
        curV = Mathf.Max(0, curV);
        curValueTxe.text = curV.ToString("F1");
        curProgress.value = curV;
        nameTxt.text = type.ToString();
    }

    public void SetTaskInfo(ObjectType objType, TaskInfo taskInfo)
    {
        info = taskInfo;
        type = objType;
    }
}
