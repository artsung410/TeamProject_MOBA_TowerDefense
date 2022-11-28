using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

[System.Serializable]
public class MinionBlueprint
{
    public string Name;                 // �̴Ͼ� �̸�
    public int ID;                      // ���� ���̵�
    public int GroupID;                 // �׷� ���̵�

    public int Rank;                    // ���
    public int Type;                    // �̴Ͼ� Ÿ��

    [Header("[����ü]")]
    public float Projectile_Speed;        // ����ü �ӵ�
    public int Projectile_Type;         // ����ü Ÿ��

    [Header("[�Ӽ�]")]
    public float Attack;                // ���ݷ�
    public float Attack_Speed;          // ���ݼӵ�
    public float Range;                 // ����
    public float Move_Speed;            // �̵��ӵ�
    public float Hp;                    // ü��
    public float Exp;                   // ����ġ
    public Sprite Icon_Blue;                 // ������
    public Sprite Icon_Red;                 // ������
}
