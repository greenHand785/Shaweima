
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;


public class FileOptionManager
{
    private static FileOptionManager instance; 
    public static FileOptionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FileOptionManager();
            }
            return instance;
        }
    }
    private FileOptionManager()
    {
        
    }

    public void SerilizealObject2XML<T>(T obj, string dirPath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(dirPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dirPath));
        }
        FileStream fs = File.Open(dirPath, FileMode.OpenOrCreate, FileAccess.Write);   
        using (MemoryStream ms = new MemoryStream())
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                xmlSerializer.Serialize(sw, obj, ns);
                byte[] bytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, bytes, 0, bytes.Length);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        fs.Close();
    }

    public T DeserilizealXML2Object<T>(string path)
    {
        if (File.Exists(path) == false)
        {
            Debug.LogError("文件不存在");
            return default;
        }
        FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T obj = (T)xmlSerializer.Deserialize(sr);
            fs.Close();
            return obj;
        }
    }

    /// <summary>
    /// 反序列化为对象 通过反射
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public object DeserilizealXML2Object(string path, Type type)
    {
        if (File.Exists(path) == false)
        {
            Debug.LogError("文件不存在");
            return default;
        }
        FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            object obj = xmlSerializer.Deserialize(sr);
            fs.Close();
            return obj;
        }
    }

    public List<T> DeserializeSheet2Object<T>(string sheetDirPath)
    {
        if (Directory.Exists(sheetDirPath) == false)
        {
            Debug.LogError("文件加不存在");
            return null;
        }
        string[] files = Directory.GetFiles(sheetDirPath);
        List<T> list = new List<T>();
        foreach (string fileName in files)
        {
            if (Path.GetExtension(fileName).Trim() != ".xml")
            {
                continue;
            }
            string path = fileName;
            T t = DeserilizealXML2Object<T>(path);
            if (t == null)
            {
                continue;
            }
            list.Add(t);
        }
        return list;
    }

    public void SerilizealObject2Json<T>(T obj, string dirPath)
    {
        string value = JsonUtility.ToJson(obj);
        FileStream fs = File.Open(dirPath, FileMode.OpenOrCreate, FileAccess.Write);
        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
        {
            sw.Write(value);
        }
        fs.Close();
    }

    public T DeserilizealJson2Object<T>(string path)
    {
        if (File.Exists(path) == false)
        {
            Debug.LogError("文件不存在");
            return default;
        }
        string jsonStr = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonStr);
    }

    // 生成消息
    public void CreateLuaNetMgrScript(string luaPath, string classNameSpace)
    {
        string fileName = Path.GetFileNameWithoutExtension(luaPath);
        string moduleManager = fileName.Replace("MsgHandles", "Manager");
        Assembly assembly = Assembly.Load("Assembly-CSharp");
        if(assembly == null)
        {
            return;
        }
        object obj = assembly.CreateInstance(classNameSpace);
        if(obj == null)
        {
            return;
        }
        Google.Protobuf.IMessage message = obj as Google.Protobuf.IMessage;
        if(message == null)
        {
            return;
        }
        StringBuilder registerMsg = new StringBuilder($"function {fileName}.RegisterNetworkMsg()\n");
        StringBuilder callBackFunc = new StringBuilder("");
        StringBuilder sendFuncs = new StringBuilder("");
        StringBuilder managerFuncs = new StringBuilder("");
        StringBuilder msgidInfo = new StringBuilder("");

        foreach (var item in message.Descriptor.Fields.InFieldNumberOrder())
        {
            string protoName = item.GetOptions().GetExtension<string>(EnumProtoExtensions.ProtoName);
            if (protoName.Contains("Response") || protoName.Contains("Notify"))
            {
                string newFuncName = fileName + "." + protoName.Replace("PBMsg", "On");
                registerMsg.AppendLine($"   {fileName}.RegisterMsg(MSGID.{item.Name}, {newFuncName});");
                string func = $"function {newFuncName}(data)\n" +
                $"  if data == nil then\n" +
                $"      return;\n" +
                $"  end\n" +
                $"  local pb =  NetworkMsgMgr.Decode(\"{item.File.Package}.{protoName}\", data);\n" +
                $"  if pb == nil then\n" +
                $"      return;\n" +
                $"  end\n" +
                $"  {moduleManager}.{protoName.Replace("PBMsg", "On")}(pb);\n" +
                $"end\n";
                string managerFunc = $"function {moduleManager}.{protoName.Replace("PBMsg", "On")}(pb_msg);\n\n" +
                    $"end\n";
                callBackFunc.AppendLine(func);
                managerFuncs.AppendLine(managerFunc);
            }
            if (protoName.Contains("Request") || protoName.Contains("Report"))
            {
                string sendFunc = $"function {fileName}.{protoName.Replace("PBMsg", "Send")}(pb_msg)\n" +
                $"  if pb_msg == nil then\n" +
                $"      return;\n" +
                $"  end\n" +
                $"  NetworkMsgMgr.SendMsg(MSGID.{item.Name}, \"{item.File.Package}.{protoName}\", pb_msg);\n" +
                $"end\n";
                sendFuncs.AppendLine(sendFunc);
            }
            string msgid = $"	{item.Name}     = {NewConvert.To16Str(item.FieldNumber)},";
            msgidInfo.AppendLine(msgid);
        }
        registerMsg.AppendLine("end\n");
        string context = $"{fileName} = {"{}"};\n\n" +
            $"local this = {fileName};\n\n" +
            registerMsg.ToString() +
            callBackFunc.ToString() +
            sendFuncs.ToString();
        string path = PanelSetting.luaDir + "/" + luaPath;
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        // 写入define中
        WriteString2FileBySpecialName(PanelSetting.luaDefinePath, msgidInfo.ToString(), "MSGID");
        // 写入manager文件中
        string managerPath = Path.GetDirectoryName(path) + $"/{moduleManager}.lua";
        WriteString2LastLine(managerPath, managerFuncs.ToString());
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(context);
            sw.Close();
            fs.Close();
            return;
        }
        File.WriteAllText(path, context);
    }

    // 生成lua页面脚本
    public void CreateLuaPanelScript(string dir, string path)
    {
        string resultPath = PanelSetting.luaDir + "/" + dir + "/" + path;
        if (!Directory.Exists(Path.GetDirectoryName(resultPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(resultPath));
        }
        if (File.Exists(resultPath))
        {
            Debug.LogError("该文件已经存在，无法创建");
            return;
        }
        string fileName = Path.GetFileNameWithoutExtension(path);
        using (FileStream fs = File.Open(resultPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            string text = $"{fileName} = {"{}"};\n" +
            $"local this = {fileName};\n" +
            " --初始化组件\n" +
$"function {fileName}.InitComponent()\n\n" +
"end\n" +
"-- 在页面生命周期中只执行一次\n" +
$"function {fileName}.OnStart(m_pid)\n" +
    "   this.m_pid = m_pid;\n" +
            $"   {fileName}.InitComponent();\n\n" +
            "end\n" +
          "-- 当页面显示或隐藏时执行\n" +
            $"function {fileName}.InitParam()\n\n" +
"end";
            byte[] arr = Encoding.UTF8.GetBytes(text);
            fs.Write(arr, 0, arr.Length);
        }
        // 将该页面信息写入
        WriteString2FileBySpecial(PanelSetting.luaPanelTypePath, $"    Panel_{Path.GetFileNameWithoutExtension(path)} = \"{Path.GetFileNameWithoutExtension(path)}\",", "");
        WriteString2FirstLine(PanelSetting.luaGameManagerPath, $"require \"{dir}/{Path.GetFileNameWithoutExtension(path)}\"");
    }

    // 创建lua panelitem
    public void CreateLuaPanelItem(string dir, string path)
    {
        string resultPath = PanelSetting.luaDir + "/" + dir + "/" + path;
        if (!Directory.Exists(Path.GetDirectoryName(resultPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(resultPath));
        }
        if (File.Exists(resultPath))
        {
            Debug.LogError("该文件已存在，无法创建");
            return;
        }
        string fileName = Path.GetFileNameWithoutExtension(path);
        using (FileStream fs = File.Open(resultPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            string text = $"{fileName} = {"{}"};\n\n"
+ $"function {fileName}.New()\n"
    + " local obj = { };\n"
            + $" {fileName}.__index = {fileName};\n"
            + $" setmetatable(obj, {fileName});\n"
            + " return obj;\n"
            + "end\n\n"
            + $"function {fileName}:Init(m_pid, sub_id)\n"
    + " self.m_pid = m_pid;\n"
            + " self.m_subpid = sub_id;\n"
            + " self.m_index = -1;\n\n"
            + "end\n\n"
  + $"function {fileName}:OnInitParam()\n\n"
+ "end";
            byte[] arr = Encoding.UTF8.GetBytes(text);
            fs.Write(arr, 0, arr.Length);
        }
    }

    // 生成页面脚本
    public void CreatePanelScript(string path)
    {
        string resultPath = Application.dataPath + "/" + Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".cs";
        if (!Directory.Exists(Path.GetDirectoryName(resultPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(resultPath));
        }
        if (File.Exists(resultPath))
        {
            Debug.LogError("该文件已经存在，无法创建");
            return;
            //   File.Delete(resultPath);
        }
        using (FileStream fs = File.Open(resultPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            string text = "using System.Collections;\n" +
                "using System.Collections.Generic;\n" +
                "using UnityEngine;\n" +
                $"public class {Path.GetFileNameWithoutExtension(path)} : PanelBase\n" +
                "{\n" +
                "   public override void InitComponent()\n" +
                "   {\n" +
                "       base.InitComponent();\n" +
                "   }\n" +
                "\n" +
                "   public override void InitParam()\n" +
                "   {\n" +
                "       base.InitParam();\n" +
                "   }\n" +
                "}\n";
            byte[] arr = Encoding.UTF8.GetBytes(text);
            fs.Write(arr, 0, arr.Length);
        }
        // 将该页面信息写入
       // WriteString2FileBySpecial(ClientSetting.PanelDefineScripPath, $"    public static string Panel_{Path.GetFileNameWithoutExtension(path)} = \"{Path.GetFileNameWithoutExtension(path)}\";");
    }

    // 生成列表项脚本
    public void CreateListItemScript(string path)
    {
        string resultPath = Application.dataPath + "/" + Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".cs";
        if (!Directory.Exists(Path.GetDirectoryName(resultPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(resultPath));
        }
        if (File.Exists(resultPath))
        {
            Debug.LogError("该文件已存在，无法创建");
            return;
           // File.Delete(resultPath);
        }
        using (FileStream fs = File.Open(resultPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            string text = "using System.Collections;\n" +
                "using System.Collections.Generic;\n" +
                "using UnityEngine;\n" +
                $"public class {Path.GetFileNameWithoutExtension(path)} : UIExampleBase\n" +
                "{\n" +
                "   public override void InitComponent()\n" +
                "   {\n" +
                "       base.InitComponent();\n" +
                "   }\n" +
                "\n" +
                "   public override void OnInitParam()\n" +
                "   {\n" +
                "       base.InitParam();\n" +
                "   }\n" +
                "}\n";
            byte[] arr = Encoding.UTF8.GetBytes(text);
            fs.Write(arr, 0, arr.Length);
        }
    }

    //向首行添加字符串
    public void WriteString2FirstLine(string filePath, string needInsert)
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        string[] lines = File.ReadAllLines(filePath);
        List<string> newLines = new List<string>();
        newLines.Add(needInsert);
        newLines.AddRange(lines);
        File.WriteAllLines(filePath, newLines);
    }

    //向首行添加字符串
    public void WriteString2LastLine(string filePath, string needInsert)
    {
        if (!File.Exists(filePath))
        {
            FileStream fs = File.Create(filePath);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(needInsert);
            sw.Close();
            fs.Close();
            return;
        }
        string[] lines = File.ReadAllLines(filePath);
        List<string> newLines = new List<string>();
        newLines.AddRange(lines);
        newLines.Add(needInsert);
        File.WriteAllLines(filePath, newLines);
    }

    // 向特定行写入字符串: 暂时只能向比较简单的脚本中写入,
    public void WriteString2FileBySpecialName(string filePath, string needInsert, string keyWord = "name")
    {
        string resultPath = Path.GetDirectoryName(filePath) + "/" + Path.GetFileName(filePath);
        if (!File.Exists(resultPath))
        {
            return;
        }
        string[] lines = File.ReadAllLines(resultPath);
        if (lines == null || lines.Length == 0)
        {
            return;
        }
        string scriptName = Path.GetFileNameWithoutExtension(resultPath);
        int lineIndex = -1;
        // 查找类定义
        for (int i = 0; i < lines.Length; i++)
        {
            if (Regex.IsMatch(lines[i], keyWord + @"\s{0,}"))
            {
                lineIndex = i;
                break;
            }
        }
        if (lineIndex == -1)
        {
            return;
        }
        // 查找右大括号
        int bigKuo = -1;
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(lines[i], @"}"))
            {
                bigKuo = i;
                break;
            }
        }
        if (bigKuo == -1)
        {
            return;
        }
        string[] newLines = new string[lines.Length + 1];
        for (int i = 0; i < newLines.Length; i++)
        {
            if (i == bigKuo)
            {
                newLines[i] = needInsert;
            }
            else if (i < bigKuo)
            {
                newLines[i] = lines[i];
            }
            else
            {
                newLines[i] = lines[i - 1];
            }
        }
        File.WriteAllLines(resultPath, newLines);
    }

    // 向特定行写入字符串: 暂时只能向比较简单的脚本中写入,
    public void WriteString2FileBySpecial(string filePath, string needInsert, string keyWord = "class")
    {
        string resultPath = Path.GetDirectoryName(filePath) + "/" + Path.GetFileName(filePath);
        if (!File.Exists(resultPath))
        {
            return;
        }
        string[] lines = File.ReadAllLines(resultPath);
        if (lines == null || lines.Length == 0)
        {
            return;
        }
        string scriptName = Path.GetFileNameWithoutExtension(resultPath);
        int lineIndex = -1;
        // 查找类定义
        for (int i = 0; i < lines.Length; i++)
        {
            if (Regex.IsMatch(lines[i], keyWord + @"\s{0,}" + scriptName))
            {
                lineIndex = i;
                break;
            }
        }
        if (lineIndex == -1)
        {
            return;
        }
        // 查找右大括号
        int bigKuo = -1;
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(lines[i], @"}"))
            {
                bigKuo = i;
                break;
            }
        }
        if (bigKuo == -1)
        {
            return;
        }
        string[] newLines = new string[lines.Length + 1];
        for (int i = 0; i < newLines.Length; i++)
        {
            if (i == bigKuo)
            {
                newLines[i] = needInsert;
            }
            else if (i < bigKuo)
            {
                newLines[i] = lines[i];
            }
            else
            {
                newLines[i] = lines[i - 1];
            }
        }
        File.WriteAllLines(resultPath, newLines);
    }

    /// <summary>
    /// 向文件写入字符串,如果原先文件存在，则会将其清空,再写入
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="text"></param>    
    public void WriteString2File(string filePath, string text, bool isClear = false)
    {
        if(Directory.Exists(Path.GetDirectoryName(filePath)) == false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
        if (!isClear)
        {
            filePath = GetUnRepeateName(filePath);
        }
        File.WriteAllText(filePath, text);
    }

    /// <summary>
    /// 向文件中写入字节流文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    public void WriteBytes2File(string filePath, byte[] data)
    {
        if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
        if(data == null || data.Length == 0)
        {
            Debug.LogError("数据为空，无需写入");
            return;
        }
        using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using(BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
            {
                bw.Write(data, 0, data.Length);
            }
        }
    }

    /// <summary>
    /// 从文件中读取字节流
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public byte[] ReadBytesFromeFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"{filePath}文件不存在");
            return null;
        }
        byte[] data = File.ReadAllBytes(filePath);
        return data;
    }

    private string GetUnRepeateName(string filePath)
    {
        int count = 0;
        string result = filePath;
        while (true)
        {
            if (File.Exists(result) == false)
            {
                return result;
            }
            string name = Path.GetFileNameWithoutExtension(filePath);
            string extend = Path.GetExtension(filePath);
            result = $"{Path.GetDirectoryName(filePath)}/{name}({count}){extend}";
            count++;
        }
    }

    /// <summary>
    /// 获得上级目录名称
    /// </summary>
    /// <returns></returns>
    public static string GetLastDictionaryName(string path)
    {
        string dic = Path.GetDirectoryName(path);
        string[] dicList = dic.Split('/');
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < dicList.Length - 1; i++)
        {
            result.Append(dicList[i] + "/");
        }
        return result.ToString();
    }

    /// <summary>
    /// 获得命名空间中最后一个
    /// </summary>
    /// <param name="space"></param>
    /// <returns></returns>
    public string GetClassFromSpace(string space)
    {
        string[] strs = space.Split('.');
        if(strs == null || strs.Length == 0)
        {
            Debug.LogError("获取错误");
            return null;
        }
        return strs[strs.Length - 1];
    }

    /// <summary>
    /// 移除文件列表中的unity .meta文件
    /// </summary>
    /// <returns></returns>
    public string[] RemoveUnityMetaFile(string[] files, string keyWord = ".meta")
    {
        if(files == null)
        {
            return null;
        }
        List<string> newStrs = new List<string>();
        foreach (var item in files)
        {
            if(Path.GetExtension(item) != keyWord)
            {
                newStrs.Add(item);
            }
        }
        return newStrs.ToArray();
    }

    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool FileExisted(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public bool DirectoryExisted(string dir)
    {
        return Directory.Exists(dir);
    }

    /// <summary>
    /// 获得文件夹中所有文件路径
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public string[] GetFiles(string dir)
    {
        return Directory.GetFiles(dir);
    }

    /// <summary>
    /// 获得proto class
    /// </summary>
    /// <returns></returns>
    public string GetProtoClassSpcace(string fullSpace)
    {
        string className = GetClassFromSpace(fullSpace);
        string[] space = fullSpace.Split('.');
        if(space == null || space.Length <= 1)
        {
            return null;
        }
        string result = "";
        for (int i = 0; i < space.Length - 1; i++)
        {
            result += space[i].ToLower() + ".";
        }
        return result + className;
    }
}
