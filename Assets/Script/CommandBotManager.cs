using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using Microsoft.Unity.VisualStudio.Editor;

public class CommandBotManager : MonoBehaviour
{
    public List<GameObject> selectedObjects = new List<GameObject>(); // 存储选中的物体
    public string selectableTagBot = "SelectableBot";                 // 可选择物体的Tag（例如机器人）
    public string selectableTag = "WorkAear";               // 目标物体的Tag（例如工作区域）

    private List<Vector3> tempWayPos=new List<Vector3>();

    public LayerMask targetLayer; // 用于指定需要检测的层级
    public float rayDistance = 100000f; // 射线的长度
    void Update()
    {
            CheckSelected();
    }

private void CheckSelected()
{
        // 鼠标左键按下：选择物体
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,rayDistance, targetLayer))
            {
                // 如果点击的是具有 "SelectableBot" 标签的物体
                if (hit.collider.CompareTag(selectableTagBot))
                {
                    GameObject selectedObject = hit.collider.gameObject;

                    // 如果按住Ctrl键，支持多选
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        // 多选模式：添加到列表
                        if (!selectedObjects.Contains(selectedObject))
                            selectedObjects.Add(selectedObject);
                    }
                    else
                    {
                        // 单独选择模式：清空之前的选中物体
                        ClearSelection(); 
                        selectedObjects.Add(selectedObject);
                    }

                    Debug.Log("选中物体：" + selectedObject.name);
                }
                else if (!hit.collider.CompareTag(selectableTag))
                { 
                    ClearSelection();  
                }
            }
            else
            {
                 ClearSelection();  
            }
        }

        // 鼠标左键松开：获取目标点并开始移动
        if (Input.GetMouseButtonUp(0) && selectedObjects.Count > 0)

        
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,rayDistance, targetLayer))
            {
                // 如果点击的是具有 "SelectableWorkArea" 标签的物体
                if (hit.collider.CompareTag(selectableTag))
                {
                    Transform target = hit.collider.transform;
                    // 任务
                    MaintenanceTask task = target.GetComponent<MaintenanceTask>();
                    WorkArea workArea= target.GetComponent<WorkArea>();
                    Transform currentWorkPoint;
                    if (task!= null) 
                    {
                        
                     foreach (GameObject selectedObject in selectedObjects)
                     {
                        tempWayPos.Clear();
                        BotBase bot = selectedObject.GetComponent<BotBase>();

                        // 尝试获取一个工作点
                        if (workArea.HasAvailablePoint())
                        {
                            if (bot.botState==BotState.Dead) continue;
                            if (!task.IsContain(bot.type))
                            {
                                  Debug.Log("无法为该任务工作：");
                                  continue;
                            } 
                            currentWorkPoint = workArea.GetWorkPoint();
                            Debug.Log("获取到工作点：" + currentWorkPoint.name);
                            bot.SetTargetTask(task,currentWorkPoint);
                        }
                        else
                        {
                            Debug.LogWarning("没有可用的工作点！");
                        }
                    }
                    }
                   ClearSelection();
                }
            }
          
        }
        UpdateSelectedBot();
}
    private void UpdateSelectedBot()
    {

        foreach (GameObject selectedObject in selectedObjects)
        {
            
			var highlighter = selectedObject.GetComponentInParent<Highlighter>();
			if (highlighter == null) { return; }
            highlighter.tween =true;    
            highlighter.overlay = true;
        }

    }    


    // 清空选中物体列表
    private void ClearSelection()
    {
       
       foreach (GameObject selectedObject in selectedObjects)
        {
            
			var highlighter = selectedObject.GetComponentInParent<Highlighter>();
			if (highlighter == null) { return; }
            highlighter.tween =false;    
            highlighter.overlay = false;
        }
        selectedObjects.Clear();
    }
}
