using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能信息
/// </summary>
public class JsonSkillInfo : JsonSheetBase
{
    public string skillName; // 技能名称
    public string skillPrefaber; // 技能预制体
    public string skillIcon;//技能图标
    public string skillDes; // 技能描述
    public int[] skillUpgradeCost;//技能升级消耗
    public int skillMaxLevel;//技能最高等级
    public float skillHarm;//技能伤害
}
