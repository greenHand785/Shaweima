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
        ResetParam();
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

    public void ResetParam()
    {
        InitHP(initHP);
        MainPanel.Instance.Init(initHP);
        damage.CurrentHealth = initHP;
    }

}
