using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 关卡信息
public class JsonLevelInfo: JsonSheetBase
{
    public int level;
    public float createTaskTime; // 创建任务时间
    public float createAniTime = 3; // 创建动画时间
    public float hurtCount = 3; // 任务惩罚
    public float taskGoldRange = 3; // 金币奖励
    public float totalTime; // 总共时间
}

