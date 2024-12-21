using System.Collections.Generic;
using UnityEngine;

public class MultiSelection : MonoBehaviour
{
    private Vector3 startMousePosition;  // 鼠标按下起点
    private Vector3 endMousePosition;    // 鼠标释放终点
    private bool isSelecting = false;    // 是否处于框选状态

    public List<GameObject> selectableObjects;  // 场景中所有可选物体
    public List<GameObject> selectedObjects;    // 当前被框选的物体

    void Update()
    {
        // 开始框选：鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startMousePosition = Input.mousePosition;
        }

        // 正在框选：鼠标左键保持按下
        if (Input.GetMouseButton(0) && isSelecting)
        {
            endMousePosition = Input.mousePosition;
        }

        // 完成框选：鼠标左键释放
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            endMousePosition = Input.mousePosition;
            SelectObjectsInRectangle();
        }
    }

    void OnGUI()
    {
        // 如果鼠标正在拖动，绘制选框
        if (isSelecting)
        {
            Rect rect = GetScreenRect(startMousePosition, endMousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));  // 半透明矩形
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f)); // 边框
        }
    }

    // 获取屏幕上的矩形
    private Rect GetScreenRect(Vector3 start, Vector3 end)
    {
        // 确保矩形坐标始终是左上角到右下角
        start.y = Screen.height - start.y;
        end.y = Screen.height - end.y;
        Vector3 topLeft = Vector3.Min(start, end);
        Vector3 bottomRight = Vector3.Max(start, end);
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    // 绘制矩形
    private void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    // 绘制矩形边框
    private void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // 上边框
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // 下边框
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        // 左边框
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // 右边框
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    // 检测框选框内的物体
    private void SelectObjectsInRectangle()
    {
        selectedObjects.Clear(); // 清空之前的选择

        // 计算屏幕矩形
        Rect selectionRect = GetScreenRect(startMousePosition, endMousePosition);

        foreach (GameObject obj in selectableObjects)
        {
            // 将物体的世界坐标转换为屏幕坐标
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(obj.transform.position);

            // 如果物体在矩形框内，将其添加到选择列表
            if (selectionRect.Contains(screenPosition, true))
            {
                selectedObjects.Add(obj);
                Debug.Log("选中物体：" + obj.name);
                 obj.GetComponent<MeshRenderer>().material.color= Color.red;
            }
        }
    }
}
