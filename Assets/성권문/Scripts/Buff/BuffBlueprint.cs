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

    [Header("[기본정보]")]
    public string Name;             // 버프 이름
    public string ToolTipName;      // 툴팁용 이름
    public int ID;                  // 고유 아이디
    public Sprite Icon;             // 버프 아이콘
    public int GroupID;             // 그룹 아이디 
    public int Rank;                // 등급
    public int Type;                // 이펙트 타입
    public int Target;              // 타겟
    public float Value;             // 이펙트 값
    public float Duration;          // 이펙트 지속시간
    public string Desc;             // 버프 설명란
}
