using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金币系统，控制玩家的金币输入输出
/// </summary>
public class GoldSystem : MonoSingleton<GoldSystem>
{
    private float m_curGold; // 当前金币

    public float Gold
    {
        set
        {
            if(m_curGold != value)
            {
                m_curGold = value;
            }
        }
        get
        {
            return m_curGold;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_curGold = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
