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
    [Header("[기본정보]")]
    public string Name;                                      // 타워 이름
    public string NickName;                                  // 타워 닉네임
    public int ID;                                           // 고유 아이디
    public int GroupID;                                      // 그룹 아이디 
    public int Rank;                                         // 등급
    public int Type;                                         // 타입
    public GameObject Pf;

    [Header("[조합]")]
    public int Combination_ResultID;                         // 조합 결과 ID
    public int Combination_Required_Value;                   // 조합 요구 카드 개수

    [Header("[뽑기]")]
    public float Normal_Random_Draw_Probability;             // 노말 랜덤 타워 뽑기 박스 확률
    public float Normal_Attack_Draw_Probability;             // 노말 공격 타워 뽑기 박스 확률
    public float Normal_Minion_Draw_Probability;             // 노말 미니언 타워 뽑기 박스 확률
    public float Normal_Buff_Debuff_Draw_Probability;        // 노말 버프/디퍼프 타워 뽑기 박스 확률

    public float Premium_Random_Draw_Probability;            // 프리미엄 랜덤 타워 뽑기 박스 확률
    public float Premium_Attack_Draw_Probability;            // 프리미엄 공격 타워 뽑기 박스 확률
    public float Premium_Minion_Draw_Probability;            // 프리미엄 미니언 타워 뽑기 박스 확률
    public float Premium_Buff_Debuff_Draw_Probability;       // 프리미엄 버프/디퍼프 타워 뽑기 박스 확률

    [Header("[속성]")]
    public float Attack;                                     // 공격력
    public float Attack_Speed;                               // 공격속도
    public int Hp;                                           // 체력
    public int Range;                                        // 범위
    public int Range_Type;                                   // 범위

    [Header("[투사체]")]
    public int Projectile_Type;                              // 투사체 타입
    public float Projectile_Speed;                           // 투사체 속도
    public GameObject Projectile_Pf;

    [Header("[부가 옵션]")]
    public GameObject Destroy_Effect_Pf;                     // 타워 파괴 이펙트
    [TextArea] public string Desc;                           // 타워 설명
    public Sprite Sprite_TowerCard;                          // 타워카드 이미지
    public Sprite Sprite_TowerProtrait;                      // 타워상태 이미지
    public AudioClip AudioClip_Attack;                     // 공격 사운드 이름
    public AudioClip AudioClip_Destroy;                        // 피격 사운드 이름

    [Header("[버프타워만]")]
    public int buffID;                                       // 버프 타워만 해당.

    [Header("[미니언타워만]")]
    public int MinionID;                                     // 미니언 타워만 해당.
}
