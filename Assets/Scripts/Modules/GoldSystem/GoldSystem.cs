using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金币系统，控制玩家的金币输入输出
/// </summary>
public class GoldSystem : MonoSingleton<GoldSystem>
{
    private float m_curGold; // 当前金币

    public int initGold = 100;
    public ObjectBase gold;
    private float m_lastYearGold;
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
        m_curGold = initGold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int Value)
    {
        m_curGold+=Value;
    }

    public void AddGold(int Value, EqupiBase equip)
    {
        m_curGold += Value;
        // 生成金币
        ObjectBase goldC = Instantiate(gold, transform);
        goldC.transform.position = equip.transform.position;
    }

    public void SubGold(int Value)
    {
        m_curGold-=Value;
        if  (m_curGold < 0) m_curGold=0;
    }

    public float GetMoneyGap()
    {
       return m_curGold-m_lastYearGold;
    }

    public void ResetParam()
    {
        m_curGold = initGold;
    }
}
