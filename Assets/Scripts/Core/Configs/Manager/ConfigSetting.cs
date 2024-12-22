using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ConfigSetting
{
    public static string ConfPath = Path.GetDirectoryName(Application.dataPath) + "/Conf"; // 配置目录，仅在编辑情况下使用
    public static string GlobalXmlPath = ConfPath + "/global_config/global_xml_config.xml"; // xml总配置文件路径
    public static string GlobalSheetPath = ConfPath + "/global_config/global_sheet_config.xml"; // sheet总配置文件路径
    public static string BytesXmlPath = Application.streamingAssetsPath + "/Configs/xml_bytes_config.xml"; // xml字节码文件配置路径
    public static string BytesSheetPath = Application.streamingAssetsPath + "/Configs/sheet_bytes_config.xml"; // sheet字节码文件配置路径
    public static string XmlEnumPath = Application.dataPath + "/Scripts/Common/Configs/Manager/XmlConfigType.cs"; // xmlType的路径
    public static string SheetEnumPath = Application.dataPath + "/Scripts/Common/Configs/Manager/SheetConfigType.cs"; // sheetType的路径
    public static string XmlPath = ConfPath + "/xml_config"; // xml配置路径
    public static string SheetPath = ConfPath + "/sheet_config"; // sheet配置路径

    public static string ConfigProtoPath = ConfPath + "/config_proto";
    public static string LuaPBPath = ConfPath + "/client/lua/config_pb"; // pb配置文件夹
}
