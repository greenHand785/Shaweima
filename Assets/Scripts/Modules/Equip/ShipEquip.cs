using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ɴ�װ��
/// </summary>
public class ShipEquip : ObjectBase
{
    public float initHP = 100;
    public ScreenDamage damage;
    private void Start()
    {
        InitHP(initHP);
        MainPanel.Instance.Init(initHP);
    }

    public override void Injured(float value)
    {
        // ����
        base.Injured(value);
        MainPanel.Instance.SetHPValue(HP);
        // ��Ļ��Ч
        damage.CurrentHealth -= value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Injured(5);
        }
    }


}
