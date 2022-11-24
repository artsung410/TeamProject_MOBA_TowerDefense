using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerSkillDatas
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    [Tooltip("��ȹ�� Ȯ�ο�")]
    public string NameLevel; // �ε��� 2

    [Tooltip("��ų �̸�/ ��ų PF")]
    public GameObject Name; // �ε��� 1

    [Tooltip("��ų ���̵�. ������ �Ľ��� ����")]
    public int ID; // �ε��� 0

    [Tooltip("ī�� �̱� Ȯ��")]
    public float Probability;

    [Tooltip("��ų ���� �Ӽ�")]
    public int Classification;

    [Tooltip("��ų�� ��ũ. 1 ~ 3�� ���� �����ϸ� ��ũ���� ���� �ٸ���")]
    public int Rank;

    [Tooltip("��ų ����� �ٸ� ��ų ��� ���� �ð�")]
    public float LockTime;

    [Tooltip("��ų ��� Ÿ��\n" +
        "1 : ���콺 ������ �ٶ󺸰� ����\n" +
        "2 : ���콺 Ŀ�� ��ġ�� �ٶ󺸰� ����ü �߻�\n" +
        "3 : ���콺 Ŀ�� ��ġ�� �ٶ󺸰� �ش� ��ġ�� �̵� �� ����\n" +
        "4 : �ؼ��� �̴Ͼ� ���귮 ����\n5 : �Ʊ� ��� �̴Ͼ� ���ݷ� / ���� �ӵ� ����\n" +
        "6 : ���콺 ��ġ�� ���� ��ȯ\n" +
        "7 : �̴Ͼ� ��ȯ")]
    public int SkillType;

    [Tooltip("Skill_Type�� ���� ����\n" +
        "1~3�� ��: ���ط�\n" +
        "4�� ��: �ؼ��� �̴Ͼ� ���귮 ������(%)\n" +
        "5�� ��: �Ʊ� �̴Ͼ� ���ݷ� ������(%)\n" +
        "6�� ��: ���ط�\n" +
        "7�� ��: ��� �� �� �� ��ȯ�Ǵ� �̴Ͼ�\n")]
    public float Value_1;

    [Tooltip("Skill_Type�� ����\n" +
        "5�� ��: ���ݼӵ� ������(%)\n" +
        "7�� ��: ���� �� �� �� ��ȯ�Ǵ� �̴Ͼ�")]
    public float Value_2;

    [Tooltip("��ų ���� �ð�")]
    public float CoolTime;

    [Tooltip("��ų�� �ִ� ��Ÿ�\n" +
        "0�̸� ��Ÿ� ���Ѿ��� ��� ����")]
    public float Range;

    [Tooltip("1 : �簢��\n" +
        "2 : ��\n" +
        "3 : ��ä��\n" +
        "4 : �� ��ü ����")]
    public int RangeType;

    [Tooltip("Range_Type �� ����\n" +
        "1�� ��: ����\n" +
        "2�� ��: ������\n" +
        "3�� ��: ������\n")]
    public float RangeValue_1;

    [Tooltip("Range_Type �� ����\n" +
        "1�� ��: ����\n" +
        "3�� ��: ���� / 2")]
    public float RangeValue_2;

    [Tooltip("��ų �����ð�/ ������ �����ִ� �ð�")]
    public float HoldingTime;

    [Tooltip("���� ���ط�")]
    public float TickDamage;

    [Tooltip("�������� Ƚ��")]
    public int TickCount;

    [Tooltip("�������� �ð�(��)")]
    public float TickTime;

    [Tooltip("�������� Ÿ��\n" +
        "1 : �̵��ӵ� ����\n" +
        "2 : ����")]
    public int CcType;

    [Tooltip("��������ȿ�� ���� �ð�")]
    public float CcTime;

    [Tooltip("���������� ��\n" +
        "CcType��\n" +
        "1�� �� : �̵��ӵ� ���� ��(%)")]
    public float CcValue;

    [Tooltip("��ų ī�� �̹���")]
    public Sprite CardImage;

    [Tooltip("�ΰ��ӻ� ��ų ������")]
    public Sprite SkillIcon;

    [Multiline(3)]
    public string Desc;
}
