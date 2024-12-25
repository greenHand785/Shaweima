using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础对象
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>
    /// 血量
    /// </summary>
    public float HP
    {
        get;
        private set;
    }

    /// <summary>
    /// 初始化血量
    /// </summary>
    /// <param name="value"></param>
    public virtual void InitHP(float value)
    {
        HP = value;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="value"></param>
    public virtual void Injured(float value)
    {
        HP -= value;
    }

    /// <summary>
    /// 治愈
    /// </summary>
    /// <param name="value"></param>
    public virtual void Cure(float value)
    {
        HP += value;
    }
}
