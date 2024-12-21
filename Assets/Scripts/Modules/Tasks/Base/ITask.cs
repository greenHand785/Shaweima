using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ӿ�
/// 1. ������
/// 2. ����ͷ�
/// 3. ������ʱ
/// 4. ��������
/// </summary>
public interface ITask
{
    /// <summary>
    /// ��ý���
    /// </summary>
    /// <returns></returns>
    public void GetRewards();

    /// <summary>
    /// ��óͷ�
    /// </summary>
    /// <returns></returns>
    public void GetPunishment();

    /// <summary>
    /// ��ʱ
    /// </summary>
    /// <returns></returns>
    public void CheckLimitedTime();

    /// <summary>
    /// ����Ƿ����ڹ���
    /// </summary>
    /// <returns></returns>
    public bool CheckIsWorking();


}
