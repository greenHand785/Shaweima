using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("ClassInfo")]
public class ConfigInfo
{
    [XmlAttribute("Class")]
    public string ClassName;
    [XmlAttribute("Path")]
    public string ConfigPath;
}
