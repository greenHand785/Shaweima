using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ConfigOption
{
    // 序列化配置文件
    [MenuItem("Config/SerializeConfig")]
    public static void SerializeConfig()
    {
        // 开始序列化xml类型配置文件
        SerializeXml();
        // 开始序列化sheet类型配置文件
        SerializeSheet();

        Debug.Log("序列化完成");
    }

    // 序列化配置文件
    [MenuItem("Config/EncodingConfig")]
    public static void EncodingAllConfig()
    {
        ConfigManager.Instance.EncodingAllData();
        Debug.Log("加密完成");
    }

    [MenuItem("Config/DecodingConfig")]
    public static void DecodingAllConfig()
    {
        ConfigManager.Instance.DecodingAllData();
        Debug.Log("解密完成");
    }

    /// <summary>
    /// 序列化xml配置文件
    /// </summary>
    private static void SerializeXml()
    {
        GlobalXmlConfig globalXmlConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalXmlConfig>(ConfigSetting.GlobalXmlPath);
        if(globalXmlConfig == null)
        {
            Debug.LogError($"序列化出现问题，请检查文件路径是否正确。路径{ConfigSetting.GlobalXmlPath}");
            return;
        }
        if(globalXmlConfig.XmlConfig == null || globalXmlConfig.XmlConfig.Count == 0)
        {
            Debug.LogError("总配置文件中不存在文件，请添加对应文件");
            return;
        }
        Assembly assembly = Assembly.Load("Assembly-CSharp");
        if(assembly == null)
        {
            Debug.LogError("程序集获取错误");
            return;
        }
        string xmlEnum = @"/// <summary>
/// 自动生成Xml配置类型，请勿手动修改
/// </summary>
public enum XmlConfigType
{
	NONE,

}";
        Dictionary<string, string> byteXmlPath = new Dictionary<string, string>();
        // 保存配置类名
        StringBuilder xmlEnumList = new StringBuilder();
        foreach (var item in globalXmlConfig.XmlConfig)
        {
            Type type = assembly.GetType(item.ClassName);
            if(type == null)
            {
                continue;
            }
            object t = FileOptionManager.Instance.DeserilizealXML2Object(ConfigSetting.XmlPath + $"/{item.ConfigPath}", type);
            string dataConfigPath = Application.streamingAssetsPath + $"/Configs/XmlConfig/{Path.GetFileNameWithoutExtension(item.ConfigPath)}.dat";
            #region 旧的序列化方式
            //MethodInfo method = type.GetMethod("Serialize");
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    method.Invoke(t, new object[] { ms, t });
            //    byte[] datas = new byte[ms.Length];
            //    Buffer.BlockCopy(ms.GetBuffer(), 0, datas, 0, datas.Length);
            //    // 将datas数据保存到streamAssets中
            //    FileOptionManager.Instance.WriteBytes2File(dataConfigPath, datas);
            //}
            #endregion

            #region 使用google官方的序列化方式
            byte[] datas = ProtoBuffEncoding.Serialize(t as Google.Protobuf.IMessage);
            if (datas == null || datas.Length == 0)
            {
                continue;
            }
            // 将datas数据保存到streamAssets中
            FileOptionManager.Instance.WriteBytes2File(dataConfigPath, datas);
            #endregion
            string configPath = $"/Configs/XmlConfig/{Path.GetFileNameWithoutExtension(item.ConfigPath)}.dat";
            // 将字节码数据路径与配置关系保存到unity配置文件中。
            if (byteXmlPath.ContainsKey(item.ClassName))
            {
                byteXmlPath[item.ClassName] = configPath;
            }
            else
            {
                byteXmlPath.Add(item.ClassName, configPath);
            }
        }
        GlobalXmlConfig bytesConfig = new GlobalXmlConfig();
        bytesConfig.XmlConfig = new List<ConfigInfo>();
        foreach (var item in byteXmlPath)
        {
            ConfigInfo info = new ConfigInfo();
            info.ClassName = item.Key;
            info.ConfigPath = item.Value;
            bytesConfig.XmlConfig.Add(info);
            xmlEnumList.Append($"\t{FileOptionManager.Instance.GetClassFromSpace(item.Key)},\n");
        }
        // 将字节流配置文件信息保存到对应配置文件中
        SaveBytesXmlConfig(bytesConfig, ConfigSetting.BytesXmlPath);
        // 将类名写入枚举类
        // 创建xml枚举
        FileOptionManager.Instance.WriteString2File(ConfigSetting.XmlEnumPath, xmlEnum, true);
        FileOptionManager.Instance.WriteString2FileBySpecial(ConfigSetting.XmlEnumPath, xmlEnumList.ToString(), "enum");
    }

    /// <summary>
    /// 将配置的字节流文件保存到对应路径
    /// </summary>
    /// <param name="config"></param>
    /// <param name="filePath"></param>
    private static void SaveBytesXmlConfig(GlobalXmlConfig bytesConfig, string filePath)
    {
        if(bytesConfig == null || bytesConfig.XmlConfig == null)
        {
            return;
        }
        FileOptionManager.Instance.SerilizealObject2XML<GlobalXmlConfig>(bytesConfig, filePath);
    }

    /// <summary>
    /// 获得类型
    /// </summary>
    /// <param name="className"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static Type GetType(string className, Assembly assembly)
    {
        if(assembly == null)
        {
            return null;
        }
        return assembly.GetType(className);
    }

    /// <summary>
    /// 序列化sheet配置文件
    /// </summary>
    private static void SerializeSheet()
    {
        GlobalSheetConfig globalSheetConfig = FileOptionManager.Instance.DeserilizealXML2Object<GlobalSheetConfig>(ConfigSetting.GlobalSheetPath);
        if(globalSheetConfig == null || globalSheetConfig.SheetConfig == null || globalSheetConfig.SheetConfig.Count == 0)
        {
            Debug.LogError($"sheet文件加载出。路径：{globalSheetConfig}");
            return;
        }
        Assembly assembly = Assembly.Load("Assembly-CSharp");
        if(assembly == null)
        {
            Debug.LogError("程序集加载错误");
            return;
        }
        string sheetEnum = @"/// <summary>
/// 自动生成sheet配置类型，请勿手动修改
/// </summary>
public enum SheetConfigType
{
    NONE,


}";
        StringBuilder sheetEnumName = new StringBuilder();
        Dictionary<string, string> sheetEnumDic = new Dictionary<string, string>();
        foreach (var item in globalSheetConfig.SheetConfig)
        {
            Type classType = assembly.GetType(item.ClassName);
            if(classType == null)
            {
                Debug.LogError("类获取错误");
                continue;
            }
            string sheetDir = ConfigSetting.SheetPath + "/" + item.ConfigPath;
            string[] sheetFiles = Directory.GetFiles(sheetDir);
            if(sheetFiles == null)
            {
                Debug.LogError("文件列表不存在");
                continue;
            }
            string dataSheetPath = Application.streamingAssetsPath + $"/Configs/SheetConfig/";
            string unityFullName;
            foreach (var sheetName in sheetFiles)
            {
                unityFullName = dataSheetPath +$"/{item.ConfigPath}/{Path.GetFileNameWithoutExtension(sheetName)}.dat";
                object t = FileOptionManager.Instance.DeserilizealXML2Object(sheetName, classType);
                byte[] data = ProtoBuffEncoding.Serialize(t as Google.Protobuf.IMessage);
                if (data == null)
                {
                    continue;
                }
                // 写入文件
                FileOptionManager.Instance.WriteBytes2File(unityFullName, data);
            }
            if (sheetEnumDic.ContainsKey(item.ClassName))
            {
                sheetEnumDic[item.ClassName] = $"/Configs/SheetConfig" + $"/{item.ConfigPath}";
            }
            else
            {
                sheetEnumDic.Add(item.ClassName, $"/Configs/SheetConfig" + $"/{item.ConfigPath}");
            }
        }
        GlobalSheetConfig byteSheet = new GlobalSheetConfig();
        byteSheet.SheetConfig = new List<ConfigInfo>();
        foreach (var item in sheetEnumDic)
        {
            sheetEnumName.Append($"\t{FileOptionManager.Instance.GetClassFromSpace(item.Key)},\n");
            ConfigInfo info = new ConfigInfo();
            info.ClassName = item.Key;
            info.ConfigPath = item.Value;
            byteSheet.SheetConfig.Add(info);
        }
        FileOptionManager.Instance.SerilizealObject2XML<GlobalSheetConfig>(byteSheet, ConfigSetting.BytesSheetPath);
        // 创建xml枚举
        FileOptionManager.Instance.WriteString2File(ConfigSetting.SheetEnumPath, sheetEnum, true);
        FileOptionManager.Instance.WriteString2FileBySpecial(ConfigSetting.SheetEnumPath, sheetEnumName.ToString(), "enum");
    }
}
