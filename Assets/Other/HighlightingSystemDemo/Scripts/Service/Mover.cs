using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{


    
    public List<Vector3>  pathPoints=new List<Vector3>(); // 路径点数组
    public Material lineMaterial;  // 线材质
    public float arrowHeadLength = 0.5f; // 箭头头部的长度
    public float arrowHeadAngle = 20f;   // 箭头头部的角度
    public float moveSpeed = 5f;   // 移动速度
    private LineRenderer lineRenderer;

    private int currentPointIndex = 0; // 当前目标点的索引

    private bool isWork = false;
    void Start()
    {
        
        Init();
    }

    void  Update()
    {
        if (!isWork) return;
        Move();
        // 绘制路径
        UpdatePath();
    }
  private void Init()
  {
        // 初始化 LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = pathPoints.Count;

  }
    void UpdatePath()
    {
        if (pathPoints == null || pathPoints.Count == 0)
            return;
        //跟新自身位置
        pathPoints[0]=transform.position;
       lineRenderer.positionCount = pathPoints.Count;
        // 设置 LineRenderer 的每个点
        for (int i = 0; i < pathPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, pathPoints[i]);
        }

        // 绘制箭头
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            DrawArrow(pathPoints[i], pathPoints[i + 1]);
        }
    }

    // 绘制箭头
    private void DrawArrow(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized; // 计算方向向量
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

        // 箭头头部的两条边
        Vector3 arrowHeadPoint1 = end + right * arrowHeadLength;
        Vector3 arrowHeadPoint2 = end + left * arrowHeadLength;

        Debug.DrawLine(end, arrowHeadPoint1, Color.green, 2f);
        Debug.DrawLine(end, arrowHeadPoint2, Color.green, 2f);
    }


  
    private void Move()
    {
        // 检查路径点是否有效
        if (pathPoints == null || pathPoints.Count == 0)
            return;

        // 获取当前目标点
        Vector3 targetPoint = pathPoints[currentPointIndex];

        // 移动物体向目标点
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        // 检查是否到达目标点
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            // 切换到下一个点
            currentPointIndex++;

            if (currentPointIndex >= pathPoints.Count)
            {
                pathPoints.Clear();
            }
        }
    }




    public void SetWork(bool value)
    {
        isWork = value;
    }

    public void SetPathPoints(List<Vector3> newPathPoints)
    {
        isWork = true;
        pathPoints.Clear();
        foreach (Vector3 point in newPathPoints)
        {
            pathPoints.Add(point);
        }
        //第一个点都是自己的位置
        currentPointIndex=0;
    }
}
