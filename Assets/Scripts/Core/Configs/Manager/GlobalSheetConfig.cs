using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Sheet类型配置信息
/// </summary>
public class GlobalSheetConfig
{
    [XmlElement("SheetConfig")]
    public List<ConfigInfo> SheetConfig;
}
