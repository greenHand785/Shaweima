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
