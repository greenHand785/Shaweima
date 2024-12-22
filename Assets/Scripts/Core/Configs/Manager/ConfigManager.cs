using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 配置加载管理工具
/// </summary>
public class ConfigManager
{
    private Dictionary<XmlConfigType, object> xmlConfigDic;
    private GlobalXmlConfig bytesXmlConfig; // bytesXmlConfig配置
    private GlobalSheetConfig bytesSheetConfig; // bytesSheetConfig配置
    private Dictionary<SheetConfigType, Dictionary<int, object>> sheetConfigDic;
    private Assembly configAssemebly; // 配置类的程序集

    private Dictionary<string, JsonConfigBase> jsonConfigDic;
    private Dictionary<string, Dictionary<int, JsonSheetBase>> jsonSheetDic;

    private string rsaPublicKey;
    private string rsaPrivateKey = @"<RSAKeyValue><Modulus>uawz9lRY0+nQ8mgVgqE2BWEt7Ae3Gwd7T3+KEM+d7vmblYKAaGYjSgKSqSH4Ma+HHPgq/zMIlI1nlEnDZXI3Bb6Spi8PJzpDT6QwxgQ6xdBGAOMQQSL/aUool/YPM8g0/RCTp67ODRx5pbGEzYdeDu5BzxbJDv/gCPoFD7WCSz0=</Modulus><Exponent>AQAB</Exponent><P>wcBvp56bFGx0joTwGD1uGsnXpsw4M+0Ksky9IayYJV41ikZBcvW4rYZtCZHOTSWi7A8B3tcsumdU9u0TxA5TKw==</P><Q>9VNJnQRumhnUhEFfJ3zd8kC5GZHqwNhFrJUfwR27PlgNAj8mdaW9BinKwxj6Y3m2DaMFHGVt4E8haGwMF1jHNw==</Q><DP>Hd5DmCx8LbbWdQ4LUUFPEvQguYbr9x1Is9neoi1QOxp92HYiKs20jZOWhY4jpoLxzcA2gprbO1UopPRBQs0Ohw==</DP><DQ>WJbU9GCPWBKPzZqVs/rBYyPAM92fejgfIO1Q5DnKTf8Z43/OcZmCIuGgmMdCU/21okMGK3TtMp0goUazLxMeDQ==</DQ><InverseQ>g0wqhCN9AcP+tCp5JHMwwJz2uYJxsdT2Cxq8XdoCZxGvkdl1jyO9Xu56ilwSof0BPkARRcUSvEtPtWNZVp4L8g==</InverseQ><D>hOosTIN3D0SLSmyOMXQ8Wr+FgzjGsHe7o15WtNAbbA6NfQrt505uprWOzbq+lLrlyywwg33B/632XyYq1X1cuuquxuTNvpzhFUmV4C2tFv1WmGCOB6Lp/hq3CIazwLrQKa8NEEROxSrDP4MbO2mLHnBTWVm3Qz+D8in9OXd2DZU=</D></RSAKeyValue>";

    private string RsaPublicKey
    {
        get
        {
            if(rsaPublicKey == null)
            {
                rsaPublicKey = File.ReadAllText(Application.streamingAssetsPath + "/Data/LockFiles/rsa_public.txt");
            }
            return rsaPublicKey;
        }
    }

    private static ConfigManager instance;
    public static ConfigManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ConfigManager();
            }
            return instance;
        }
    }

    public ConfigManager()
    {
        jsonConfigDic = new Dictionary<string, JsonConfigBase>();
        jsonSheetDic = new Dictionary<string, Dictionary<int, JsonSheetBase>>();
    }
    
    /// <summary>
    /// 加载xml类型配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    public T GetXmlConfig<T>(XmlConfigType type) where T : class, Google.Protobuf.IMessage, new()
    {
        if(xmlConfigDic == null)
        {
            xmlConfigDic = new Dictionary<XmlConfigType, object>();
        }
        xmlConfigDic.TryGetValue(type, out object obj);
        if(obj != null)
        {
            return obj as T;
        }
        string shortClassName = type.ToString();
        string fullClassName = GetXmlFullClassName(shortClassName);
        string xmlPath = Application.streamingAssetsPath + "/" + GetXmlFilePath(fullClassName);
        byte[] data = FileOptionManager.Instance.ReadBytesFromeFile(xmlPath);
        obj = ProtoBuffEncoding.Deserialize<T>(data);
        xmlConfigDic.Add(type, obj);
        return obj as T;
    }

    /// <summary>
    /// 从xml配置列表中获得文件路径
    /// </summary>
    /// <returns></returns>
    public string GetXmlFilePath(string className)
    {
        if(bytesXmlConfig == null)
        {
            bytesXmlConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalXmlConfig>(ConfigSetting.BytesXmlPath);
        }
        if(bytesXmlConfig == null || bytesXmlConfig.XmlConfig == null)
        {
            Debug.LogError("错误：xml配置文件不存在");
            return null;
        }
        foreach (var item in bytesXmlConfig.XmlConfig)
        {
            if(item.ClassName == className)
            {
                return item.ConfigPath;
            }
        }
        return null;
    }

    /// <summary>
    /// 获得完成类名称
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public string GetXmlFullClassName(string className)
    {
        if (bytesXmlConfig == null)
        {
            bytesXmlConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalXmlConfig>(ConfigSetting.BytesXmlPath);
        }
        if (bytesXmlConfig == null || bytesXmlConfig.XmlConfig == null)
        {
            Debug.LogError("错误：xml配置文件不存在");
            return null;
        }
        foreach (var item in bytesXmlConfig.XmlConfig)
        {
            if (FileOptionManager.Instance.GetClassFromSpace(item.ClassName) == className)
            {
                return item.ClassName;
            }
        }
        return null;
    }

    /// <summary>
    /// 加载sheet类型配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public T GetSheetConfig<T>(SheetConfigType type, int id) where T : class, Google.Protobuf.IMessage, new ()
    {
        if(sheetConfigDic == null)
        {
            sheetConfigDic = new Dictionary<SheetConfigType, Dictionary<int, object>>();
        }
        ;
        if(sheetConfigDic.TryGetValue(type, out Dictionary<int, object> list))
        {
            if (list.TryGetValue(id, out object obj))
            {
                return obj as T;
            }
            else
            {
                return null;
            }
        }
        // 根据反射加载配置文件
        if(configAssemebly == null)
        {
            configAssemebly = Assembly.Load("Assembly-CSharp");
        }
        string shortClassName = type.ToString();
        string fullClassName = GetSheetFullClassName(shortClassName);
        Type classType = configAssemebly.GetType(fullClassName);
        if(classType == null)
        {
            Debug.LogError("类名有错误，请检查配置是否正确");
            return null;
        }
        PropertyInfo idField = classType.GetProperty("Id");
        if(idField == null)
        {
            Debug.LogError("无法找到对应id");
            return null;
        }
        list = new Dictionary<int, object>();
        sheetConfigDic.Add(type, list);
        string path = Application.streamingAssetsPath + "/" + GetSheetDicPath(fullClassName);
        string[] sheets = FileOptionManager.Instance.RemoveUnityMetaFile(Directory.GetFiles(path));
        T newObj = null;
        int newID = -1;
        T resultObj = null;
        byte[] data = null;
        foreach (var realFilePath in sheets)
        {
            data = FileOptionManager.Instance.ReadBytesFromeFile(realFilePath);
            if(data == null)
            {
                continue;
            }
            newObj = ProtoBuffEncoding.Deserialize<T>(data);
            if(newObj == null)
            {
                continue;
            }
            newID = (int)idField.GetValue(newObj);
            if (list.ContainsKey(newID))
            {
                list[newID] = newObj;
            }
            else
            {
                list.Add(newID, newObj);
            }
            if(newID == id)
            {
                resultObj = newObj;
            }
        }
        return resultObj;
    }

    /// <summary>
    /// 获得sheet配置类的全称
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public string GetSheetFullClassName(string className)
    {
        if (bytesSheetConfig == null)
        {
            bytesSheetConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalSheetConfig>(ConfigSetting.BytesSheetPath);
        }
        if (bytesSheetConfig == null || bytesSheetConfig.SheetConfig == null)
        {
            Debug.LogError("错误：xml配置文件不存在");
            return null;
        }
        foreach (var item in bytesSheetConfig.SheetConfig)
        {
            if (FileOptionManager.Instance.GetClassFromSpace(item.ClassName) == className)
            {
                return item.ClassName;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据类信息获得对应配置的文件路径
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public string GetSheetDicPath(string className)
    {
        if (bytesSheetConfig == null)
        {
            bytesSheetConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalSheetConfig>(ConfigSetting.BytesSheetPath);
        }
        if (bytesSheetConfig == null || bytesSheetConfig.SheetConfig == null)
        {
            Debug.LogError("错误：xml配置文件不存在");
            return null;
        }
        foreach (var item in bytesSheetConfig.SheetConfig)
        {
            if (item.ClassName == className)
            {
                return item.ConfigPath;
            }
        }
        return null;
    }

    /// <summary>
    /// 获得json配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T GetJsonConfig<T>(string type) where T : JsonConfigBase
    {
        if (!File.Exists(type))
        {
            Debug.LogError(type + "配置不存在");
            return null;
        }
        if(jsonConfigDic == null)
        {
            jsonConfigDic = new Dictionary<string, JsonConfigBase>();
        }
        if(!jsonConfigDic.TryGetValue(type, out JsonConfigBase config))
        {
            //config = JsonUtility.FromJson<T>(File.ReadAllText(type));
            config = JsonConvert.DeserializeObject<T>(Decoding(File.ReadAllText(type)));
            jsonConfigDic.Add(type, config);
        }
        return config as T;
    }

    /// <summary>
    /// 获得sheet配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetJsonSheetConfig<T>(string type, int id) where T : JsonSheetBase
    {
        if (!Directory.Exists(type))
        {
            Debug.LogError(type + "配置不存在");
            return null;
        }
        if(jsonSheetDic == null)
        {
            jsonSheetDic = new Dictionary<string, Dictionary<int, JsonSheetBase>>();
        }
        if(!jsonSheetDic.TryGetValue(type, out Dictionary<int, JsonSheetBase> sheets))
        {
            sheets = new Dictionary<int, JsonSheetBase>();
            jsonSheetDic.Add(type, sheets);
            if (!Directory.Exists(type))
            {
                Directory.CreateDirectory(type);
            }
            string[] sheetNames = Directory.GetFiles(type);
            foreach (var sheetContent in sheetNames)
            {
                if(Path.GetExtension(sheetContent) != ".json")
                {
                    continue;
                }
                T sheet = JsonUtility.FromJson<T>(Decoding(File.ReadAllText(sheetContent)));
                if (!sheets.ContainsKey(sheet.ID))
                {
                    sheets.Add(sheet.ID, sheet);
                }
            }
        }
        sheets.TryGetValue(id, out JsonSheetBase value);
        return value as T;
    }

    /// <summary>
    /// 保存jsonConfig
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    public void SetJsonConfig<T>(string type, T value = null) where T : JsonConfigBase
    {
        if(jsonConfigDic.TryGetValue(type, out JsonConfigBase config))
        {
            if(value != null)
            {
                jsonConfigDic[type] = value;
                config = value;
            }
        }
        else
        {
            jsonConfigDic.Add(type, value);
            config = value;
        }
        //string json = Encoding(JsonConvert.SerializeObject(config));
        string json = Encoding(JsonUtility.ToJson(config));
        if (!Directory.Exists(Path.GetDirectoryName(type)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(type));
        }
        File.WriteAllText(type, json);
    }

    /// <summary>
    /// 保存jsonSheet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void SetJsonSheet<T>(string type, int id, T value = null) where T : JsonSheetBase
    {
        if(!jsonSheetDic.TryGetValue(type, out Dictionary<int, JsonSheetBase> sheets))
        {
            sheets = new Dictionary<int, JsonSheetBase>();
            jsonSheetDic.Add(type, sheets);
        }
        if(sheets.TryGetValue(id, out JsonSheetBase sheet))
        {
            if(value != null)
            {
                sheets[id] = value;
                sheet = value;
                value.ID = id;
            }
        }
        else
        {
            sheets.Add(id, value);
            sheet = value;
        }
        if(sheet == null)
        {
            return;
        }
        sheet.ID = id;
        string json = Encoding(JsonUtility.ToJson(sheet));
        if (!Directory.Exists(type))
        {
            Directory.CreateDirectory(type);
        }
        File.WriteAllText(type + $"/{Path.GetFileName(type)}_{id}.json", json);
    }

    /// <summary>
    /// 删除josnConfig
    /// </summary>
    /// <param name="type"></param>
    public void DeleteJsonConfig(string type)
    {
        if (jsonConfigDic.ContainsKey(type))
        {
            jsonConfigDic.Remove(type);
        }
        if (File.Exists(type))
        {
            File.Delete(type);
        }
    }

    /// <summary>
    /// 删除jsonSheet
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void DeleteJsonSheet(string type, int id)
    {
        if(jsonSheetDic.TryGetValue(type, out Dictionary<int, JsonSheetBase> sheets))
        {
            if (sheets.ContainsKey(id))
            {
                sheets.Remove(id);
                string filePath = type + $"/{Path.GetFileName(type)}_{id}.json";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }

    /// <summary>
    /// 获得唯一id
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetJsonSheetUniqueID<T>(string type) where T : JsonSheetBase
    {
        //初始化jsonSheetDic
        if(!jsonSheetDic.TryGetValue(type, out Dictionary<int, JsonSheetBase> sheets))
        {
            sheets = new Dictionary<int, JsonSheetBase>();
            jsonSheetDic.Add(type, sheets);
            if (!Directory.Exists(type))
            {
                Directory.CreateDirectory(type);
            }
            string[] sheetNames = Directory.GetFiles(type);
            foreach (var sheetContent in sheetNames)
            {
                if(Path.GetExtension(sheetContent) != ".json")
                {
                    continue;
                }
                T sheet = JsonUtility.FromJson<T>(File.ReadAllText(sheetContent));
                if (!sheets.ContainsKey(sheet.ID))
                {
                    sheets.Add(sheet.ID, sheet);
                }
            }
        }
            


        if(jsonSheetDic.TryGetValue(type, out Dictionary<int, JsonSheetBase> sheets_2))
        {
            int value = sheets_2.Count;
            while (sheets_2.ContainsKey(value))
            {
                value++;
            }
            return value;
        }
        else
        {
            sheets = new Dictionary<int, JsonSheetBase>();
            jsonSheetDic.Add(type, sheets);
            if (!Directory.Exists(type))
            {
                Directory.CreateDirectory(type);
            }
            string[] sheetNames = Directory.GetFiles(type);
            foreach (var sheetContent in sheetNames)
            {
                if (Path.GetExtension(sheetContent) != ".json")
                {
                    continue;
                }
                T sheet = JsonUtility.FromJson<T>(File.ReadAllText(sheetContent));
                if (!sheets.ContainsKey(sheet.ID))
                {
                    sheets.Add(sheet.ID, sheet);
                }
            }
            return GetJsonSheetUniqueID<T>(type);
        }
    }

    /// <summary>
    /// 获得JsonSheetName
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetJsonSheetName(string type, int id)
    {
        return $"{Path.GetFileName(type)}_{id}";
    }

    /// <summary>
    /// 创建某sheet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T CreateJsonSheet<T>(string type) where T : JsonSheetBase, new()
    {
        int id = GetJsonSheetUniqueID<T>(type);
        T t = new T();
        t.ID = id;
        SetJsonSheet(type, id, t);
        return t;
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string Encoding(string content)
    {
        //return RSAEndoing.RSAEncrypt(RsaPublicKey, content);
        return content;
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string Decoding(string str)
    {
        //return RSAEndoing.RSADecrypt(rsaPrivateKey, str);
        return str;
    }

    public void EncodingAllData()
    {
        FieldInfo[] list = typeof(JsonConfigType).GetFields();
        JsonConfigType info = new JsonConfigType();
        foreach (var item in list)
        {
            string path = (string)item.GetValue(info);
            string value = File.ReadAllText(path);
            File.WriteAllText(path, Encoding(value));
        }
        FieldInfo[] list2 = typeof(JsonSheetType).GetFields();
        JsonSheetType info2 = new JsonSheetType();
        foreach (var item in list2)
        {
            string dirPath = (string)item.GetValue(info2);
            string[] files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".json")
                {
                    continue;
                }
                string value = File.ReadAllText(file);
                File.WriteAllText(file, Encoding(value));
            }
        }
    }

    public void DecodingAllData()
    {
        FieldInfo[] list = typeof(JsonConfigType).GetFields();
        JsonConfigType info = new JsonConfigType();
        foreach (var item in list)
        {
            string path = (string)item.GetValue(info);
            string value = File.ReadAllText(path);
            File.WriteAllText(path, Decoding(value));
        }
        FieldInfo[] list2 = typeof(JsonSheetType).GetFields();
        JsonSheetType info2 = new JsonSheetType();
        foreach (var item in list2)
        {
            string dirPath = (string)item.GetValue(info2);
            string[] files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".json")
                {
                    continue;
                }
                string value = File.ReadAllText(file);
                File.WriteAllText(file, Decoding(value));
            }
        }
    }
}
