using System.Collections;
using UnityEngine;

public class Gold : MonoBehaviour
{
   
    public int  GoldValue=2;


    public float shakeAmount = 0.1f; // 抖动幅度
    public float shakeDuration = 0.5f; // 抖动持续时长
    public float flySpeed = 5f; // 飞向金库的速度

    private bool isFlying = false; // 是否正在飞向金库
    private Vector3 originalPosition; // 金块的初始位置

    void Start()
    {
        originalPosition = transform.position; // 记录初始位置
    }

    void Update()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0) && !isFlying)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject) // 判断是否点击了当前金块
                {
                    StartCoroutine(HandleClick()); // 处理点击后的动作
                }
            }
        }
    }


    

    /// <summary>
    /// 被点击时的处理逻辑
    /// </summary>
    private IEnumerator HandleClick()
    {
        yield return StartCoroutine(Shake()); // 先执行抖动效果
        yield return StartCoroutine(FlyToVault()); // 再飞向金库
    }

    /// <summary>
    /// 抖动效果
    /// </summary>
    private IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            // 随机偏移金块的位置
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount)
            );

            transform.position = originalPosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 恢复到原始位置
        transform.position = originalPosition;
    }

    /// <summary>
    /// 金块飞向金库
    /// </summary>
    private IEnumerator FlyToVault()
    {
        isFlying = true;

        while (Vector3.Distance(transform.position, GoldSystem.Instance.transform.position) > 0.1f)
        {
            // 插值移动金块位置
            transform.position = Vector3.MoveTowards(transform.position, GoldSystem.Instance.transform.position, flySpeed * Time.deltaTime);
            yield return null;
        }

        // 确保最终位置精确对齐到金库
        transform.position = GoldSystem.Instance.transform.position ;

        Debug.Log($"{gameObject.name} 被拾取！");
        GoldSystem.Instance.AddGold(GoldValue);
        // 金块到达金库后执行的逻辑（可以销毁或隐藏）
        Destroy(this.gameObject); // 示例：销毁金块
        isFlying = false;
    }

    
}
