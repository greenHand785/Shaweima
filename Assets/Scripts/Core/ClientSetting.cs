using UnityEngine;


public class ClientSetting
{
    public static string XmlPath = Application.streamingAssetsPath + "/Configs/XmlConfig"; // 每个模块唯一的配置文件路径
    public static string SheetPath = Application.streamingAssetsPath + "/Configs/SheetConfig"; // 多个配置文件路径
    public static string JsonConfigRoot = Application.streamingAssetsPath + "/Configs/JsonConfig";
    public static string JsonSheetRoot = Application.streamingAssetsPath + "/Configs/JsonSheet";
}