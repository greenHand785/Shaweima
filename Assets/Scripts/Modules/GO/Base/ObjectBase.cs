using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>
    /// Ѫ��
    /// </summary>
    public float HP
    {
        get;
        private set;
    }

    public float initHP2;
    /// <summary>
    /// ��ʼ��Ѫ��
    /// </summary>
    /// <param name="value"></param>
    public virtual void InitHP(float value)
    {
        HP = value;
        initHP2 = value;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="value"></param>
    public virtual void Injured(float value)
    {
        HP -= value;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="value"></param>
    public virtual void Cure(float value)
    {
        HP += value;
    }
}
