using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 全局xml类型配置信息
/// </summary>
public class GlobalXmlConfig
{
    [XmlElement("XmlConfig")]
    public List<ConfigInfo> XmlConfig;
}
