using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerSkillDatas
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    [Tooltip("기획자 확인용")]
    public string NameLevel; // 인덱스 2

    [Tooltip("스킬 이름/ 스킬 PF")]
    public GameObject Name; // 인덱스 1

    [Tooltip("스킬 아이디. 데이터 파싱할 기준")]
    public int ID; // 인덱스 0

    [Tooltip("카드 뽑기 확률")]
    public float Probability;

    [Tooltip("스킬 직업 속성")]
    public int Classification;

    [Tooltip("스킬의 랭크. 1 ~ 3성 까지 존재하며 랭크별로 값이 다르다")]
    public int Rank;

    [Tooltip("스킬 사용중 다른 스킬 사용 방지 시간")]
    public float LockTime;

    [Tooltip("스킬 사용 타입\n" +
        "1 : 마우스 방향을 바라보고 시전\n" +
        "2 : 마우스 커서 위치를 바라보고 투사체 발사\n" +
        "3 : 마우스 커서 위치를 바라보고 해당 위치로 이동 후 시전\n" +
        "4 : 넥서스 미니언 생산량 증가\n5 : 아군 모든 미니언 공격력 / 공격 속도 증가\n" +
        "6 : 마우스 위치에 도형 소환\n" +
        "7 : 미니언 소환")]
    public int SkillType;

    [Tooltip("Skill_Type의 값에 따라\n" +
        "1~3일 때: 피해량\n" +
        "4일 떄: 넥서스 미니언 생산량 증가량(%)\n" +
        "5일 때: 아군 미니언 공격력 증가량(%)\n" +
        "6일 때: 피해량\n" +
        "7일 때: 블루 팀 일 때 소환되는 미니언\n")]
    public float Value_1;

    [Tooltip("Skill_Type의 값이\n" +
        "5일 때: 공격속도 증가량(%)\n" +
        "7일 때: 레드 팀 일 때 소환되는 미니언")]
    public float Value_2;

    [Tooltip("스킬 재사용 시간")]
    public float CoolTime;

    [Tooltip("스킬의 최대 사거리\n" +
        "0이면 사거리 제한없이 사용 가능")]
    public float Range;

    [Tooltip("1 : 사각형\n" +
        "2 : 원\n" +
        "3 : 부채꼴\n" +
        "4 : 맵 전체 적용")]
    public int RangeType;

    [Tooltip("Range_Type 의 값이\n" +
        "1일 때: 가로\n" +
        "2일 때: 반지름\n" +
        "3일 때: 반지름\n")]
    public float RangeValue_1;

    [Tooltip("Range_Type 의 값이\n" +
        "1일 때: 세로\n" +
        "3일 때: 각도 / 2")]
    public float RangeValue_2;

    [Tooltip("스킬 유지시간/ 도형이 남아있는 시간")]
    public float HoldingTime;

    [Tooltip("지속 피해량")]
    public float TickDamage;

    [Tooltip("지속피해 횟수")]
    public int TickCount;

    [Tooltip("지속피해 시간(초)")]
    public float TickTime;

    [Tooltip("군중제어 타입\n" +
        "1 : 이동속도 감소\n" +
        "2 : 스턴")]
    public int CcType;

    [Tooltip("군중제어효과 유지 시간")]
    public float CcTime;

    [Tooltip("군중제어기술 값\n" +
        "CcType이\n" +
        "1일 때 : 이동속도 감소 량(%)")]
    public float CcValue;

    [Tooltip("스킬 카드 이미지")]
    public Sprite CardImage;

    [Tooltip("인게임상 스킬 아이콘")]
    public Sprite SkillIcon;

    [Multiline(3)]
    public string Desc;
}
