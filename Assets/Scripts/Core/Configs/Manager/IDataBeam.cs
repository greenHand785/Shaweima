
public class DataBeamBase
{
    /// <summary>
    /// 从配置T中加载数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="config"></param>
    public virtual void SerializeFromConfig(JsonSheetBase config)
    {
        if (config == null)
        {
            return;
        }
    }

    /// <summary>
    /// 将数据写入配置T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="config"></param>
    public virtual void Save2Config(out JsonSheetBase config)
    {
        config = new JsonSheetBase();
    }

    /// <summary>
    /// 从配置T中加载数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheet"></param>
    public virtual void SerializeFromSheet(JsonSheetBase sheet)
    {
        if(sheet == null)
        {
            return;
        }

    }

    /// <summary>
    /// 将数据写入配置T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheet"></param>
    /// <returns></returns>
    public virtual void Save2Sheet(out JsonSheetBase sheet)
    {
        sheet = new JsonSheetBase();
    }
}

