using System.Collections.Generic;
using UnityEngine;

public class WorkArea : MonoBehaviour
{
    public List<Transform> allWorkPoints = new List<Transform>(); // 所有的工作点
    private List<Transform> availableWorkPoints = new List<Transform>(); // 可用的工作点
    private HashSet<Transform> occupiedWorkPoints = new HashSet<Transform>(); // 已占用的工作点

    void Start()
    {
        Init();
    }

    void Init()
    {
        // 初始化时，所有工作点默认都可用
        availableWorkPoints = new List<Transform>(allWorkPoints);
    }

    /// <summary>
    /// 检查是否还有可用的工作点
    /// </summary>
    public bool HasAvailablePoint()
    {
        return availableWorkPoints.Count > 0;
    }

    /// <summary>
    /// 获取一个可用的工作点，并将其标记为占用
    /// </summary>
    public Transform GetWorkPoint()
    {
        if (availableWorkPoints.Count <= 0)
        {
            Debug.LogWarning("没有可用的工作点！");
            return null;
        }

        // 获取第一个可用的工作点
        Transform workPoint = availableWorkPoints[0];

        // 将该点从可用列表移除，并添加到占用列表
        availableWorkPoints.Remove(workPoint);
        occupiedWorkPoints.Add(workPoint);

        return workPoint;
    }

    /// <summary>
    /// 释放一个已占用的工作点，将其返回到可用列表
    /// </summary>
    public void ReleaseWorkPoint(Transform workPoint)
    {
        if (workPoint == null)
        {
            Debug.LogWarning("释放的工作点不能为空！");
            return;
        }

        if (occupiedWorkPoints.Contains(workPoint))
        {
            // 从占用列表中移除
            occupiedWorkPoints.Remove(workPoint);

            // 添加回可用列表
            availableWorkPoints.Add(workPoint);
        }
        else
        {
            Debug.LogWarning("尝试释放未被占用的工作点！");
        }
    }

    /// <summary>
    /// 获取当前已经被占用的工作点数量
    /// </summary>
    public int GetOccupiedCount()
    {
        return occupiedWorkPoints.Count;
    }

    /// <summary>
    /// 获取当前可用的工作点数量
    /// </summary>
    public int GetAvailableCount()
    {
        return availableWorkPoints.Count;
    }
}
