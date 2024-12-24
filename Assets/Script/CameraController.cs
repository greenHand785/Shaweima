using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 相机需要观察的目标点
    public float distance = 5.0f; // 初始距离目标点的距离
    public float zoomSpeed = 2.0f; // 滚轮缩放速度
    public float rotationSpeed = 5.0f; // 鼠标旋转速度
    public float minDistance = 1.0f; // 缩放的最小距离
    public float maxDistance = 10.0f; // 缩放的最大距离

    private float currentX = 0.0f; // 当前水平旋转角度
    private float currentY = 0.0f; // 当前垂直旋转角度
    public float yMinLimit = -30f; // 垂直旋转的最小角度
    public float yMaxLimit = 60f;  // 垂直旋转的最大角度

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("请为 CameraController 分配一个目标点！");
            return;
        }

        // 初始化相机的旋转角度
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 鼠标右键拖动旋转视角
        if (Input.GetMouseButton(1)) // 右键
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // 限制垂直旋转角度
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }

        // 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // 根据旋转角度计算相机位置
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0); // 计算旋转角度
        Vector3 direction = new Vector3(0, 0, -distance); // 计算距离
        Vector3 position = rotation * direction + target.position; // 计算最终相机位置

        // 设置相机的位置和旋转
        transform.position = position;
        transform.LookAt(target); // 让相机始终看向目标点
    }
}
