using UnityEngine;

public class ShipFade : MonoBehaviour
{
    [Range(0f, 1f)] // 显示滑动条，范围为 0 到 1
    public float transparency = 1f; // 透明度（1 表示完全不透明，0 表示完全透明）

    public Transform targetTransform; // 需要操作的目标 Transform

    private Renderer[] renderers; // 存储目标下的所有 Renderer

    void Start()
    {
        // 确保在开始时获取所有的 Renderer
        if (targetTransform != null)
        {
            renderers = targetTransform.GetComponentsInChildren<Renderer>();
            SetMaterialsToTransparent(); // 初始化时将材质设置为透明模式
        }
        else
        {
            Debug.LogWarning("请为 ShipFade 分配一个有效的 Transform！");
        }
    }

    private void Update()
    {
        // 动态设置透明度
        if (renderers != null && renderers.Length > 0)
        {
            UpdateTransparency(transparency);
        }
    }

    /// <summary>
    /// 将所有材质转换为透明模式
    /// </summary>
    private void SetMaterialsToTransparent()
    {
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                // 检查材质是否已经是透明模式
                if (material.HasProperty("_Color"))
                {
                    SetMaterialToTransparent(material);
                }
            }
        }
    }

    /// <summary>
    /// 动态更新透明度
    /// </summary>
    /// <param name="alpha">透明度值 (0~1)</param>
    private void UpdateTransparency(float alpha)
    {
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    Color color = material.color;
                    color.a = alpha; // 设置透明度
                    material.color = color;
                }
            }
        }
    }

    /// <summary>
    /// 设置材质为透明模式
    /// </summary>
    /// <param name="material">需要修改的材质</param>
    private void SetMaterialToTransparent(Material material)
    {
        // 设置渲染模式为透明
        material.SetFloat("_Mode", 3); // Unity 的标准 Shader 使用 3 表示透明模式
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}
