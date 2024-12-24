using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
public class CommandBotManager : MonoBehaviour
{
    public List<GameObject> selectedObjects = new List<GameObject>(); // 存储选中的物体
    public string selectableTagBot = "SelectableBot";                 // 可选择物体的Tag（例如机器人）
    public string selectableTag = "WorkAear";                        // 目标物体的Tag（例如工作区域）

    public LayerMask targetLayer;                                    // 用于指定需要检测的层级
    public float rayDistance = 100000f;                              // 射线的长度

    public LineRenderer lineRenderer;                                // 用于绘制框选线框

    private Vector2 startPos;                                        // 框选起点
    private Vector2 endPos;                                          // 框选终点
    private bool isSelecting = false;                                // 是否处于框选状态

    private Material selectionMaterial;                              // 用于绘制框选的材质

    void Start()
    {
        // 初始化LineRenderer
        lineRenderer.positionCount = 5; // 4条边 + 闭合线
        lineRenderer.loop = false;
        lineRenderer.enabled = false;

        // 给LineRenderer设置颜色和材质（可选）
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;
    }

    void Update()
    {
        HandleSelectionBox();     // 处理框选逻辑
        CheckSelected();          // 检测单击选择逻辑
    }

    private void HandleSelectionBox()
    {
        // 鼠标左键按下：开始框选
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isSelecting = true;
            lineRenderer.enabled = true;
        }

        // 鼠标左键拖动：更新框选线框
        if (Input.GetMouseButton(0) && isSelecting)
        {
            endPos = Input.mousePosition;
            UpdateLineRenderer();
            SelectObjectsInBox();
        }

        // 鼠标左键松开：完成框选
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            lineRenderer.enabled = false;
            //SelectObjectsInBox();
        }
    }

    private void UpdateLineRenderer()
    {
        // 设置屏幕坐标的四个角点位置
        Vector3[] corners = new Vector3[5];
        corners[0] = Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, Camera.main.nearClipPlane));
        corners[1] = Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, endPos.y, Camera.main.nearClipPlane));
        corners[2] = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y, Camera.main.nearClipPlane));
        corners[3] = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, startPos.y, Camera.main.nearClipPlane));
        corners[4] = corners[0]; // 闭合矩形

        // 设置LineRenderer的点
        lineRenderer.positionCount = 5;
        lineRenderer.SetPositions(corners);
    }

    private void SelectObjectsInBox()
    {
        // 框选区域的矩形
        Rect selectionRect = new Rect(
            Mathf.Min(startPos.x, endPos.x),
            Mathf.Min(startPos.y, endPos.y),
            Mathf.Abs(startPos.x - endPos.x),
            Mathf.Abs(startPos.y - endPos.y)
        );

        // 遍历所有带有 "SelectableBot" 标签的物体
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(selectableTagBot))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
            if (selectionRect.Contains(screenPos)) // 如果物体在框选区域内
            {
                if (!selectedObjects.Contains(obj))
                {
                    selectedObjects.Add(obj);
                }
            }
        }

        UpdateSelectedBot();
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
                        //tempWayPos.Clear();
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
        // 更新选中物体的状态（例如高亮显示）
        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject == null) continue;
            var highlighter = selectedObject.GetComponentInParent<Highlighter>();
            if (highlighter == null) { return; }
            highlighter.tween = true;
            highlighter.overlay = true;
        }
    }

    private void ClearSelection()
    {
        // 清空选中物体列表
        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject!=null)
            {
                var highlighter = selectedObject.GetComponentInParent<Highlighter>();
                if (highlighter == null) { return; }
                highlighter.tween = false;
                highlighter.overlay = false;
            }
        }
        selectedObjects.Clear();
    }
}
