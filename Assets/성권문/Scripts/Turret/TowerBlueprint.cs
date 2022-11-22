using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################


[System.Serializable]
public class TowerBlueprint
{
    [Header("[�⺻����]")]
    public int ID;                                           // ���� ���̵�
    //public string Pf_Name;                                   // Ÿ�� ������ �̸�
    public GameObject Pf;
    public string Name;                                      // Ÿ�� �̸�

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
    public int GroupID;                                      // �׷� ���̵� 
    public int Rank;                                         // ���
    public int Type;                                         // Ÿ��
    public float Attack;                                     // ���ݷ�
    public float Attack_Speed;                               // ���ݼӵ�
    public int Hp;                                           // ü��
    public int Range;                                        // ����
    public int Range_Type;                                   // ����

    [Header("[����ü]")]
    public int Projectile_Type;                              // ����ü Ÿ��
    public float Projectile_Speed;                           // ����ü �ӵ�
    //public string Projectile_Pf_Name;                        // ����ü ������ �̸�
    public GameObject Projectile_Pf;

    [Header("[�ΰ��ɼ�]")]
    public string Destroy_Effect_Pf_Name;                    // Ÿ�� �ı� ����Ʈ
    public string Desc;                                      // Ÿ�� ����
    public Sprite Sprite_TowerCard;                     // Ÿ��ī�� �̹���
    public Sprite Sprite_TowerProtrait;                 // Ÿ������ �̹���
    public string AudioClip_Attack_Name;                     // ���� ���� �̸�
    public string AudioClip_Hit_Name;                        // �ǰ� ���� �̸�
    public string AudioClip_Normal_Name;                     // ���� ���� �̸�
}
