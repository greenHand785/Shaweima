using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务接口
/// 1. 任务奖励
/// 2. 任务惩罚
/// 3. 任务限时
/// 4. 任务需求
/// </summary>
public interface ITask
{
    /// <summary>
    /// 获得奖励
    /// </summary>
    /// <returns></returns>
    public void GetRewards();

    /// <summary>
    /// 获得惩罚
    /// </summary>
    /// <returns></returns>
    public void GetPunishment();

    /// <summary>
    /// 限时
    /// </summary>
    /// <returns></returns>
    public void CheckLimitedTime();

    /// <summary>
    /// 检测是否正在工作
    /// </summary>
    /// <returns></returns>
    public bool CheckIsWorking();


}
