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

    [Tooltip("출력캐릭터 이름")]
    public string name;

    [Tooltip("Name_Level")]
    public string nameLevel;

    [Tooltip("체력")]
    public float hp;

    [Tooltip("공격력")]
    public float damage;

    [Tooltip("공격 사거리")]
    public float range;

    [Tooltip("공격 속도")]
    public float attackSpeed;

    [Tooltip("이동속도")]
    public float moveSpeed;

    [Tooltip("레벨업에 필요한 경험치")]
    public int maxExp;

    [Tooltip("레벨업 후 ID")]
    public int charID;

    [Tooltip("해당 캐릭터 처치시 얻는 경험치")]
    public int expEnemy;
}
