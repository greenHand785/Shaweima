using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 产品等级等级表
/// </summary>
public class JsonProductList : JsonSheetBase
{
    public List<JsonProductDefield> list;
}

// 创建速度等级表
[Serializable]
public class JsonProductDefield
{
    public int level; // 等级
    public string path; // 预制体路径
    public float createTotalTime; // 生成总耗时s
    public float moveSpeed; // 移动速度
    public float effect; // 效率
    public float cost; // 升级到下一级消耗金币数量
    public float durability; // 耐久度
    public string iconPath; // 机器人头像图片
    public string upIconPath; // 升级图片
    public Color workingProcessCol; // 工作颜色
    public Color headCol; // 头像
}
