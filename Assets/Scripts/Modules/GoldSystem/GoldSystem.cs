using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ϵͳ��������ҵĽ���������
/// </summary>
public class GoldSystem : MonoSingleton<GoldSystem>
{
    private float m_curGold; // ��ǰ���

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
