using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ϣ
/// </summary>
public class JsonSkillInfo : JsonSheetBase
{
    public string skillName; // ��������
    public string skillPrefaber; // ����Ԥ����
    public string skillIcon;//����ͼ��
    public string skillDes; // ��������
    public int[] skillUpgradeCost;//������������
    public int skillMaxLevel;//������ߵȼ�
    public float skillHarm;//�����˺�
}
