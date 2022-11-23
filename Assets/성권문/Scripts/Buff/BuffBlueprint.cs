using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuffBlueprint
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    [Header("[�⺻����]")]
    public int ID;                  // ���� ���̵�
    public string Name;             // ���� �̸�
    public Sprite Icon;             // ���� ������
    public int GroupID;             // �׷� ���̵� 
    public int Rank;                // ���
    public int Type;                // ����Ʈ Ÿ��
    public int Target;              // Ÿ��
    public float Value;             // ����Ʈ ��
    public float Duration;          // ����Ʈ ���ӽð�
}
