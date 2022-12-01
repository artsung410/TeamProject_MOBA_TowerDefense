using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public enum Tower_Type
{
    Attack_Tower = 1,
    Buff_Tower,
    DeBuff_Tower,
    Minion_Tower
}

[System.Serializable]
public class TowerBlueprint
{
    [Header("[�⺻����]")]
    public string Name;                                      // Ÿ�� �̸�
    public string NickName;                                  // Ÿ�� �г���
    public int ID;                                           // ���� ���̵�
    public int GroupID;                                      // �׷� ���̵� 
    public int Rank;                                         // ���
    public int Type;                                         // Ÿ��
    public GameObject Pf;

    [Header("[����]")]
    public int Combination_ResultID;                         // ���� ��� ID
    public int Combination_Required_Value;                   // ���� �䱸 ī�� ����

    [Header("[�̱�]")]
    public float Normal_Random_Draw_Probability;             // �븻 ���� Ÿ�� �̱� �ڽ� Ȯ��
    public float Normal_Attack_Draw_Probability;             // �븻 ���� Ÿ�� �̱� �ڽ� Ȯ��
    public float Normal_Minion_Draw_Probability;             // �븻 �̴Ͼ� Ÿ�� �̱� �ڽ� Ȯ��
    public float Normal_Buff_Debuff_Draw_Probability;        // �븻 ����/������ Ÿ�� �̱� �ڽ� Ȯ��

    public float Premium_Random_Draw_Probability;            // �����̾� ���� Ÿ�� �̱� �ڽ� Ȯ��
    public float Premium_Attack_Draw_Probability;            // �����̾� ���� Ÿ�� �̱� �ڽ� Ȯ��
    public float Premium_Minion_Draw_Probability;            // �����̾� �̴Ͼ� Ÿ�� �̱� �ڽ� Ȯ��
    public float Premium_Buff_Debuff_Draw_Probability;       // �����̾� ����/������ Ÿ�� �̱� �ڽ� Ȯ��

    [Header("[�Ӽ�]")]
    public float Attack;                                     // ���ݷ�
    public float Attack_Speed;                               // ���ݼӵ�
    public int Hp;                                           // ü��
    public int Range;                                        // ����
    public int Range_Type;                                   // ����

    [Header("[����ü]")]
    public int Projectile_Type;                              // ����ü Ÿ��
    public float Projectile_Speed;                           // ����ü �ӵ�
    public GameObject Projectile_Pf;

    [Header("[�ΰ� �ɼ�]")]
    public GameObject Destroy_Effect_Pf;                     // Ÿ�� �ı� ����Ʈ
    [TextArea] public string Desc;                           // Ÿ�� ����
    public Sprite Sprite_TowerCard;                          // Ÿ��ī�� �̹���
    public Sprite Sprite_TowerProtrait;                      // Ÿ������ �̹���
    public AudioClip AudioClip_Attack;                     // ���� ���� �̸�
    public AudioClip AudioClip_Destroy;                        // �ǰ� ���� �̸�

    [Header("[����Ÿ����]")]
    public int buffID;                                       // ���� Ÿ���� �ش�.

    [Header("[�̴Ͼ�Ÿ����]")]
    public int MinionID;                                     // �̴Ͼ� Ÿ���� �ش�.
}
