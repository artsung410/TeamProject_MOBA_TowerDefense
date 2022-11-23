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

    [Header("[기본정보]")]
    public int ID;                  // 고유 아이디
    public string Name;             // 버프 이름
    public Sprite Icon;             // 버프 아이콘
    public int GroupID;             // 그룹 아이디 
    public int Rank;                // 등급
    public int Type;                // 이펙트 타입
    public int Target;              // 타겟
    public float Value;             // 이펙트 값
    public float Duration;          // 이펙트 지속시간
}
