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
    public string Name;                 // 미니언 이름
    public int ID;                      // 고유 아이디
    public int GroupID;                 // 그룹 아이디

    public int Rank;                    // 등급
    public int Type;                    // 미니언 타입

    [Header("[투사체]")]
    public float Projectile_Speed;        // 투사체 속도
    public int Projectile_Type;         // 투사체 타입

    [Header("[속성]")]
    public float Attack;                // 공격력
    public float Attack_Speed;          // 공격속도
    public float Range;                 // 범위
    public float Move_Speed;            // 이동속도
    public float Hp;                    // 체력
    public float Exp;                   // 경험치
    public Sprite Icon_Blue;                 // 아이콘
    public Sprite Icon_Red;                 // 아이콘
}
