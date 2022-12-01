using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff_Target
{
    Player = 1,
    Enemy,
    My_Minion,
    Enemy_Minion,
    Whole_Enemy,
    Tower
}

public enum Buff_Group
{
    Attack_Increase = 1,
    HP_Regen_Increase,
    Move_Speed_Increase,
    Attack_Speed_Increase,
    Attack_Decrese,
    HP_Regen_Decrease,
    Move_Speed_Decrese,
    Attack_Speed_Decrese,
    Stun,
    Freezing,
    Burn,
    Airborne,
    Knockback,
}

[System.Serializable]
public class BuffBlueprint
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    [Header("[�⺻����]")]
    public string Name;             // ���� �̸�
    public string ToolTipName;      // ������ �̸�
    public int ID;                  // ���� ���̵�
    public Sprite Icon;             // ���� ������
    public int GroupID;             // �׷� ���̵� 
    public int Rank;                // ���
    public int Type;                // ����Ʈ Ÿ��
    public int Target;              // Ÿ��
    public float Value;             // ����Ʈ ��
    public float Duration;          // ����Ʈ ���ӽð�
    public string Desc;             // ���� �����
}
