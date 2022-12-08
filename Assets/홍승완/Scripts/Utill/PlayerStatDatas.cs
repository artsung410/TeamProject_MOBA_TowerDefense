using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatDatas
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public int id;

    [Tooltip("���ĳ���� �̸�")]
    public string name;

    [Tooltip("Name_Level")]
    public string nameLevel;

    [Tooltip("ü��")]
    public float hp;

    [Tooltip("���ݷ�")]
    public float damage;

    [Tooltip("���� ��Ÿ�")]
    public float range;

    [Tooltip("���� �ӵ�")]
    public float attackSpeed;

    [Tooltip("�̵��ӵ�")]
    public float moveSpeed;

    [Tooltip("�������� �ʿ��� ����ġ")]
    public int maxExp;

    [Tooltip("������ �� ID")]
    public int charID;

    [Tooltip("�ش� ĳ���� óġ�� ��� ����ġ")]
    public int expEnemy;
}
