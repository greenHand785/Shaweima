using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 机器人工厂
/// 1. 升级机器人
/// 2. 生产机器人
/// </summary>
public class BotFactory : MonoSingleton<BotFactory>
{
    private Dictionary<ObjectType, BotFactoryInfoBase> botExamples;

    public Transform botRoot; // 机器人根目录

    // Start is called before the first frame update
    void Start()
    {
        AddBotExample(ObjectType.BotA, 1, 1);
        AddBotExample(ObjectType.BotB, 2, 1);
        AddBotExample(ObjectType.BotC, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        CheckProduce();
    }

    /// <summary>
    /// 检测生产
    /// </summary>
    private void CheckProduce()
    {
        if(botExamples == null)
        {
            return;
        }
        float deltalTime = Time.deltaTime;
        foreach (var item in botExamples)
        {
            item.Value.curTime += deltalTime;
            item.Value.isProduce = item.Value.curTime >= item.Value.info.createTotalTime;
        }
    }

    /// <summary>
    /// 添加机器人模版
    /// </summary>
    /// <param name="type"></param>
    /// <param name="levelId"></param>
    /// <param name="curLevel"></param>
    public void AddBotExample(ObjectType type, int levelId, int curLevel)
    {
        if (botExamples == null)
        {
            botExamples = new Dictionary<ObjectType, BotFactoryInfoBase>();
        }
        if(botExamples.ContainsKey(type))
        {
            Debug.LogError("已经包含该模版，无需重复添加");
            return;
        }
        JsonProductDefield info = GetJsonProductInfo(levelId, curLevel);
        if(info == null)
        {
            Debug.LogError("模版不存在，无法添加");
            return;
        }
        BotFactoryInfoBase value = new BotFactoryInfoBase();
        value.info = info;
        value.levelId = levelId;
        botExamples.Add(type, value);
    }

    /// <summary>
    /// 获得升级具体某个升级属性
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="curLevel"></param>
    /// <returns></returns>
    public JsonProductDefield GetJsonProductInfo(int listId, int curLevel)
    {
        JsonProductList levelList = ConfigManager.Instance.GetJsonSheetConfig<JsonProductList>(JsonSheetType.Json_BuoyWayPointsSheet, listId);
        if (levelList == null || levelList.list == null || levelList.list.Count <= 0)
        {
            Debug.LogError("模版不存在，无法添加");
            return null;
        }
        JsonProductDefield info = null;
        foreach (var item in levelList.list)
        {
            if (item.level == curLevel)
            {
                info = item;
                break;
            }
        }
        return info;
    }

    /// <summary>
    /// 添加机器人模版
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    public void AddBotExample<T>(ObjectType type, T value) where T : BotFactoryInfoBase
    {
        if(value == null)
        {
            Debug.LogError("模版不存在，无法添加");
            return;
        }
        if(botExamples == null)
        {
            botExamples = new Dictionary<ObjectType, BotFactoryInfoBase>();
        }
        if (botExamples.ContainsKey(type))
        {
            Debug.LogError("已经包含该模版，无需重复添加");
            return;
        }
        botExamples.Add(type, value);
    }

    /// <summary>
    /// 升级产品等级
    /// </summary>
    public void UpProductLevel(ObjectType type, GoldSystem gold)
    {
        if(botExamples == null ||!botExamples.ContainsKey(type))
        {
            Debug.LogError("机器人不存在，无法升级");
            return;
        }
        if(botExamples.TryGetValue(type, out BotFactoryInfoBase value))
        {
            if(gold.Gold < value.info.cost)
            {
                Debug.LogError("金币不够，无法升级");
                return;
            }
            JsonProductDefield info = GetJsonProductInfo(value.levelId, value.info.level + 1);
            if(info == null)
            {
                Debug.LogError("等级不存在，无法升级");
                return;
            }
            gold.Gold -= value.info.cost;
            value.info = info;
            Debug.Log("升级成功");
        }
    }

    /// <summary>
    /// 生产机器人
    /// </summary>
    /// <param name="type"></param>
    public void CreateProduct(ObjectType type)
    {
        if (botExamples == null || !botExamples.ContainsKey(type))
        {
            Debug.LogError("机器人不存在，无法创建");
            return;
        }
        if(botExamples.TryGetValue(type, out BotFactoryInfoBase value))
        {
            GameObject go = Instantiate(Resources.Load<GameObject>(value.info.path));
            go.transform.SetParent(botRoot, false);
            go.transform.localPosition = Vector3.zero;
            // 创建成功!
            Debug.Log("创建成功");
            value.curTime = 0;
        }
    }

    /// <summary>
    /// 机器人对象
    /// </summary>
    /// <returns></returns>
    public Dictionary<ObjectType, BotFactoryInfoBase> GetBotExamples()
    {
        return botExamples;
    }
}
