using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����˹���
/// 1. ����������
/// 2. ����������
/// </summary>
public class BotFactory : MonoSingleton<BotFactory>
{
    private Dictionary<ObjectType, BotFactoryInfoBase> botExamples;

    public Transform botRoot; // �����˸�Ŀ¼

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
    /// �������
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
    /// ��ӻ�����ģ��
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
            Debug.LogError("�Ѿ�������ģ�棬�����ظ����");
            return;
        }
        JsonProductDefield info = GetJsonProductInfo(levelId, curLevel);
        if(info == null)
        {
            Debug.LogError("ģ�治���ڣ��޷����");
            return;
        }
        BotFactoryInfoBase value = new BotFactoryInfoBase();
        value.info = info;
        value.levelId = levelId;
        botExamples.Add(type, value);
    }

    /// <summary>
    /// �����������ĳ����������
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="curLevel"></param>
    /// <returns></returns>
    public JsonProductDefield GetJsonProductInfo(int listId, int curLevel)
    {
        JsonProductList levelList = ConfigManager.Instance.GetJsonSheetConfig<JsonProductList>(JsonSheetType.Json_BuoyWayPointsSheet, listId);
        if (levelList == null || levelList.list == null || levelList.list.Count <= 0)
        {
            Debug.LogError("ģ�治���ڣ��޷����");
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
    /// ��ӻ�����ģ��
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    public void AddBotExample<T>(ObjectType type, T value) where T : BotFactoryInfoBase
    {
        if(value == null)
        {
            Debug.LogError("ģ�治���ڣ��޷����");
            return;
        }
        if(botExamples == null)
        {
            botExamples = new Dictionary<ObjectType, BotFactoryInfoBase>();
        }
        if (botExamples.ContainsKey(type))
        {
            Debug.LogError("�Ѿ�������ģ�棬�����ظ����");
            return;
        }
        botExamples.Add(type, value);
    }

    /// <summary>
    /// ������Ʒ�ȼ�
    /// </summary>
    public void UpProductLevel(ObjectType type, GoldSystem gold)
    {
        if(botExamples == null ||!botExamples.ContainsKey(type))
        {
            Debug.LogError("�����˲����ڣ��޷�����");
            return;
        }
        if(botExamples.TryGetValue(type, out BotFactoryInfoBase value))
        {
            if(gold.Gold < value.info.cost)
            {
                Debug.LogError("��Ҳ������޷�����");
                return;
            }
            JsonProductDefield info = GetJsonProductInfo(value.levelId, value.info.level + 1);
            if(info == null)
            {
                Debug.LogError("�ȼ������ڣ��޷�����");
                return;
            }
            gold.Gold -= value.info.cost;
            value.info = info;
            Debug.Log("�����ɹ�");
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="type"></param>
    public void CreateProduct(ObjectType type)
    {
        if (botExamples == null || !botExamples.ContainsKey(type))
        {
            Debug.LogError("�����˲����ڣ��޷�����");
            return;
        }
        if(botExamples.TryGetValue(type, out BotFactoryInfoBase value))
        {
            GameObject go = Instantiate(Resources.Load<GameObject>(value.info.path));
            go.transform.SetParent(botRoot, false);
            go.transform.localPosition = Vector3.zero;
            // �����ɹ�!
            Debug.Log("�����ɹ�");
            value.curTime = 0;
        }
    }

    /// <summary>
    /// �����˶���
    /// </summary>
    /// <returns></returns>
    public Dictionary<ObjectType, BotFactoryInfoBase> GetBotExamples()
    {
        return botExamples;
    }
}
